using EcoTech.Datos;
using EcoTech.Entidades;
using EcoTech.Negocio;
using EcoTech.Utilidades;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace EcoTech.Presentacion
{
    public partial class FrmProductos : Form
    {
        private readonly N_Productos negocio =
            new N_Productos();

        private readonly Conexion conexion =
            new Conexion();

        private int idProductoSeleccionado = 0;

        public FrmProductos()
        {
            InitializeComponent();

            ConfigurarFormulario();
            CargarCategorias();
            CargarProductos();
            LimpiarCampos();

            btnGuardar.Click += btnGuardar_Click;
            btnActualizar.Click += btnActualizar_Click;
            btnDesactivar.Click += btnDesactivar_Click;
            btnLimpiar.Click += btnLimpiar_Click;

            dgvProductos.CellClick += dgvProductos_CellClick;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
        }

        private void ConfigurarFormulario()
        {
            Text = "EcoTech - Gestión de Productos";
            StartPosition = FormStartPosition.CenterScreen;

            chkActivo.Checked = true;

            txtStock.ReadOnly = true;

            dgvProductos.AutoGenerateColumns = true;
            dgvProductos.ReadOnly = true;
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.MultiSelect = false;
            dgvProductos.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CargarCategorias()
        {
            try
            {
                cmbCategoria.Items.Clear();

                using (var cn = conexion.ObtenerConexion())
                {
                    cn.Open();

                    string sql = @"
                        SELECT
                            id,
                            nombre
                        FROM dbo.categorias
                        ORDER BY nombre;
                    ";

                    using (var cmd =
                           new System.Data.SqlClient.SqlCommand(
                               sql,
                               cn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbCategoria.Items.Add(
                                new CategoriaItem
                                {
                                    Id = Convert.ToInt32(
                                        reader["id"]),

                                    Nombre = reader["nombre"]
                                        .ToString()
                                });
                        }
                    }
                }

                cmbCategoria.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar las categorías.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CargarProductos()
        {
            try
            {
                dgvProductos.DataSource =
                    negocio.Listar();

                ConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar los productos.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnas()
        {
            if (dgvProductos.Columns["Id"] != null)
                dgvProductos.Columns["Id"]
                    .HeaderText = "ID";

            if (dgvProductos.Columns["Codigo"] != null)
                dgvProductos.Columns["Codigo"]
                    .HeaderText = "Código";

            if (dgvProductos.Columns["Nombre"] != null)
                dgvProductos.Columns["Nombre"]
                    .HeaderText = "Producto";

            if (dgvProductos.Columns["CategoriaId"] != null)
                dgvProductos.Columns["CategoriaId"]
                    .Visible = false;

            if (dgvProductos.Columns["CategoriaNombre"] != null)
                dgvProductos.Columns["CategoriaNombre"]
                    .HeaderText = "Categoría";

            if (dgvProductos.Columns["PrecioCosto"] != null)
            {
                dgvProductos.Columns["PrecioCosto"]
                    .HeaderText = "Precio costo";

                dgvProductos.Columns["PrecioCosto"]
                    .DefaultCellStyle.Format = "N2";
            }

            if (dgvProductos.Columns["PrecioVenta"] != null)
            {
                dgvProductos.Columns["PrecioVenta"]
                    .HeaderText = "Precio venta";

                dgvProductos.Columns["PrecioVenta"]
                    .DefaultCellStyle.Format = "N2";
            }

            if (dgvProductos.Columns["Stock"] != null)
            {
                dgvProductos.Columns["Stock"]
                    .HeaderText = "Stock";

                dgvProductos.Columns["Stock"]
                    .DefaultCellStyle.Format = "N2";
            }

            if (dgvProductos.Columns["StockMinimo"] != null)
            {
                dgvProductos.Columns["StockMinimo"]
                    .HeaderText = "Stock mínimo";

                dgvProductos.Columns["StockMinimo"]
                    .DefaultCellStyle.Format = "N2";
            }

            if (dgvProductos.Columns["Activo"] != null)
                dgvProductos.Columns["Activo"]
                    .HeaderText = "Activo";

            if (dgvProductos.Columns["FechaRegistro"] != null)
            {
                dgvProductos.Columns["FechaRegistro"]
                    .HeaderText = "Fecha de registro";

                dgvProductos.Columns["FechaRegistro"]
                    .DefaultCellStyle.Format =
                    "dd/MM/yyyy HH:mm";
            }
        }

        private void BuscarProductos()
        {
            try
            {
                string texto =
                    txtBuscar.Text.Trim();

                List<Producto> productos =
                    negocio.Listar();

                if (!string.IsNullOrWhiteSpace(texto))
                {
                    productos = productos.FindAll(producto =>
                        producto.Codigo.IndexOf(
                            texto,
                            StringComparison.OrdinalIgnoreCase) >= 0
                        ||
                        producto.Nombre.IndexOf(
                            texto,
                            StringComparison.OrdinalIgnoreCase) >= 0
                        ||
                        (!string.IsNullOrWhiteSpace(
                            producto.CategoriaNombre) &&
                         producto.CategoriaNombre.IndexOf(
                             texto,
                             StringComparison.OrdinalIgnoreCase) >= 0));
                }

                dgvProductos.DataSource = productos;

                ConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron buscar los productos.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private bool ObtenerDecimal(
            string texto,
            string nombreCampo,
            out decimal valor)
        {
            valor = 0;

            if (string.IsNullOrWhiteSpace(texto))
            {
                MessageBox.Show(
                    $"Debe introducir {nombreCampo}.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            if (!decimal.TryParse(
                texto,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out valor))
            {
                MessageBox.Show(
                    $"El valor de {nombreCampo} no es válido.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            return true;
        }

        private bool ValidarProducto(
            out decimal precioCosto,
            out decimal precioVenta,
            out decimal stockMinimo)
        {
            precioCosto = 0;
            precioVenta = 0;
            stockMinimo = 0;

            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show(
                    "El código del producto es obligatorio.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtCodigo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show(
                    "El nombre del producto es obligatorio.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtNombre.Focus();
                return false;
            }

            if (cmbCategoria.SelectedItem == null)
            {
                MessageBox.Show(
                    "Debe seleccionar una categoría.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbCategoria.Focus();
                return false;
            }

            if (!ObtenerDecimal(
                txtPrecioCosto.Text,
                "el precio de costo",
                out precioCosto))
            {
                txtPrecioCosto.Focus();
                return false;
            }

            if (!ObtenerDecimal(
                txtPrecioVenta.Text,
                "el precio de venta",
                out precioVenta))
            {
                txtPrecioVenta.Focus();
                return false;
            }

            if (!ObtenerDecimal(
                txtStockMinimo.Text,
                "el stock mínimo",
                out stockMinimo))
            {
                txtStockMinimo.Focus();
                return false;
            }

            if (precioCosto < 0)
            {
                MessageBox.Show(
                    "El precio de costo no puede ser negativo.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            if (precioVenta < 0)
            {
                MessageBox.Show(
                    "El precio de venta no puede ser negativo.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            if (stockMinimo < 0)
            {
                MessageBox.Show(
                    "El stock mínimo no puede ser negativo.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            return true;
        }

        private void GuardarProducto()
        {
            try
            {
                if (!ValidarProducto(
                    out decimal precioCosto,
                    out decimal precioVenta,
                    out decimal stockMinimo))
                {
                    return;
                }

                Producto producto =
                    new Producto
                    {
                        Codigo =
                            txtCodigo.Text.Trim(),

                        Nombre =
                            txtNombre.Text.Trim(),

                        CategoriaId =
                            ((CategoriaItem)
                                cmbCategoria.SelectedItem).Id,

                        PrecioCosto =
                            precioCosto,

                        PrecioVenta =
                            precioVenta,

                        // Un producto nuevo comienza
                        // con stock 0.
                        Stock = 0,

                        StockMinimo =
                            stockMinimo,

                        Activo =
                            chkActivo.Checked
                    };

                negocio.Insertar(producto);

                MessageBox.Show(
                    "Producto registrado correctamente.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarProductos();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo guardar el producto.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ActualizarProducto()
        {
            try
            {
                if (idProductoSeleccionado <= 0)
                {
                    MessageBox.Show(
                        "Seleccione un producto.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                if (!ValidarProducto(
                    out decimal precioCosto,
                    out decimal precioVenta,
                    out decimal stockMinimo))
                {
                    return;
                }

                CategoriaItem categoria =
                    (CategoriaItem)
                        cmbCategoria.SelectedItem;

                Producto producto =
                    new Producto
                    {
                        Id =
                            idProductoSeleccionado,

                        Codigo =
                            txtCodigo.Text.Trim(),

                        Nombre =
                            txtNombre.Text.Trim(),

                        CategoriaId =
                            categoria.Id,

                        PrecioCosto =
                            precioCosto,

                        PrecioVenta =
                            precioVenta,

                        StockMinimo =
                            stockMinimo,

                        Activo =
                            chkActivo.Checked
                    };

                negocio.Editar(producto);

                MessageBox.Show(
                    "Producto actualizado correctamente.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarProductos();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo actualizar el producto.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void DesactivarProducto()
        {
            try
            {
                if (idProductoSeleccionado <= 0)
                {
                    MessageBox.Show(
                        "Seleccione un producto.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                DialogResult resultado =
                    MessageBox.Show(
                        "¿Está seguro de desactivar este producto?",
                        "Confirmar",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                if (resultado != DialogResult.Yes)
                    return;

                negocio.Desactivar(
                    idProductoSeleccionado);

                MessageBox.Show(
                    "Producto desactivado correctamente.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarProductos();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo desactivar el producto.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            idProductoSeleccionado = 0;

            txtCodigo.Clear();
            txtNombre.Clear();
            txtPrecioCosto.Clear();
            txtPrecioVenta.Clear();
            txtStock.Clear();
            txtStockMinimo.Clear();

            cmbCategoria.SelectedIndex = -1;

            chkActivo.Checked = true;

            dgvProductos.ClearSelection();

            txtCodigo.Focus();
        }

        private void dgvProductos_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;

                DataGridViewRow fila =
                    dgvProductos.Rows[e.RowIndex];

                idProductoSeleccionado =
                    Convert.ToInt32(
                        fila.Cells["Id"].Value);

                txtCodigo.Text =
                    fila.Cells["Codigo"].Value
                    ?.ToString() ?? string.Empty;

                txtNombre.Text =
                    fila.Cells["Nombre"].Value
                    ?.ToString() ?? string.Empty;

                int categoriaId =
                    Convert.ToInt32(
                        fila.Cells["CategoriaId"].Value);

                for (int i = 0;
                     i < cmbCategoria.Items.Count;
                     i++)
                {
                    CategoriaItem categoria =
                        (CategoriaItem)
                            cmbCategoria.Items[i];

                    if (categoria.Id == categoriaId)
                    {
                        cmbCategoria.SelectedIndex = i;
                        break;
                    }
                }

                txtPrecioCosto.Text =
                    Convert.ToDecimal(
                        fila.Cells["PrecioCosto"].Value)
                    .ToString("0.00");

                txtPrecioVenta.Text =
                    Convert.ToDecimal(
                        fila.Cells["PrecioVenta"].Value)
                    .ToString("0.00");

                txtStock.Text =
                    Convert.ToDecimal(
                        fila.Cells["Stock"].Value)
                    .ToString("0.00");

                txtStockMinimo.Text =
                    Convert.ToDecimal(
                        fila.Cells["StockMinimo"].Value)
                    .ToString("0.00");

                chkActivo.Checked =
                    Convert.ToBoolean(
                        fila.Cells["Activo"].Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo seleccionar el producto.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void txtBuscar_TextChanged(
            object sender,
            EventArgs e)
        {
            BuscarProductos();
        }

        private void btnGuardar_Click(
            object sender,
            EventArgs e)
        {
            GuardarProducto();
        }

        private void btnActualizar_Click(
            object sender,
            EventArgs e)
        {
            ActualizarProducto();
        }

        private void btnDesactivar_Click(
            object sender,
            EventArgs e)
        {
            DesactivarProducto();
        }

        private void btnLimpiar_Click(
            object sender,
            EventArgs e)
        {
            LimpiarCampos();
        }
    }

    public class CategoriaItem
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }
}