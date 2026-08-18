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
    public partial class FrmInventario : Form
    {
        private readonly N_Inventario negocio =
            new N_Inventario();

        private readonly Conexion conexion =
            new Conexion();

        private List<ProductoInventario> productos =
            new List<ProductoInventario>();

        private int usuarioActualId = 0;

        public FrmInventario(int usuarioId)
        {
            InitializeComponent();

            usuarioActualId = usuarioId;

            ConfigurarFormulario();
            CargarProductos();
            CargarUsuarios();
            CargarTipos();
            CargarMovimientos();

            btnRegistrar.Click += btnRegistrar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            cmbProducto.SelectedIndexChanged +=
                cmbProducto_SelectedIndexChanged;
            cmbTipo.SelectedIndexChanged +=
                cmbTipo_SelectedIndexChanged;
            txtCantidad.TextChanged +=
                txtCantidad_TextChanged;
        }

        private void ConfigurarFormulario()
        {
            Text = "EcoTech - Movimientos de Inventario";
            StartPosition = FormStartPosition.CenterScreen;

            txtStockActual.ReadOnly = true;

            dgvMovimientos.AutoGenerateColumns = true;
            dgvMovimientos.ReadOnly = true;
            dgvMovimientos.AllowUserToAddRows = false;
            dgvMovimientos.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvMovimientos.MultiSelect = false;
            dgvMovimientos.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CargarProductos()
        {
            try
            {
                productos = negocio.ListarProductos();

                cmbProducto.Items.Clear();

                foreach (ProductoInventario producto in productos)
                {
                    cmbProducto.Items.Add(producto);
                }

                cmbProducto.SelectedIndex = -1;
                txtStockActual.Clear();
                lblStockNuevo.Text = "Stock nuevo: 0.00";
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

        private void CargarUsuarios()
        {
            try
            {
                cmbUsuario.Items.Clear();

                using (var cn = conexion.ObtenerConexion())
                {
                    cn.Open();

                    string sql = @"
                        SELECT
                            id,
                            usuario
                        FROM dbo.usuarios
                        WHERE activo = 1
                        ORDER BY usuario;
                    ";

                    using (var cmd =
                           new System.Data.SqlClient.SqlCommand(
                               sql,
                               cn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbUsuario.Items.Add(
                                new UsuarioInventarioItem
                                {
                                    Id = Convert.ToInt32(
                                        reader["id"]),

                                    Nombre =
                                        reader["usuario"].ToString()
                                });
                        }
                    }
                }

                for (int i = 0;
                     i < cmbUsuario.Items.Count;
                     i++)
                {
                    UsuarioInventarioItem usuario =
                        (UsuarioInventarioItem)
                            cmbUsuario.Items[i];

                    if (usuario.Id == usuarioActualId)
                    {
                        cmbUsuario.SelectedIndex = i;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar los usuarios.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CargarTipos()
        {
            cmbTipo.Items.Clear();

            cmbTipo.Items.Add("Entrada");
            cmbTipo.Items.Add("Salida");
            cmbTipo.Items.Add("Ajuste");

            cmbTipo.SelectedIndex = 0;
        }

        private void CargarMovimientos()
        {
            try
            {
                dgvMovimientos.DataSource =
                    negocio.ListarMovimientos();

                ConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar los movimientos.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnas()
        {
            if (dgvMovimientos.Columns["Id"] != null)
                dgvMovimientos.Columns["Id"]
                    .HeaderText = "ID";

            if (dgvMovimientos.Columns["ProductoId"] != null)
                dgvMovimientos.Columns["ProductoId"]
                    .Visible = false;

            if (dgvMovimientos.Columns["UsuarioId"] != null)
                dgvMovimientos.Columns["UsuarioId"]
                    .Visible = false;

            if (dgvMovimientos.Columns["ProductoCodigo"] != null)
                dgvMovimientos.Columns["ProductoCodigo"]
                    .HeaderText = "Código";

            if (dgvMovimientos.Columns["ProductoNombre"] != null)
                dgvMovimientos.Columns["ProductoNombre"]
                    .HeaderText = "Producto";

            if (dgvMovimientos.Columns["UsuarioNombre"] != null)
                dgvMovimientos.Columns["UsuarioNombre"]
                    .HeaderText = "Usuario";

            if (dgvMovimientos.Columns["Tipo"] != null)
                dgvMovimientos.Columns["Tipo"]
                    .HeaderText = "Tipo";

            if (dgvMovimientos.Columns["Cantidad"] != null)
            {
                dgvMovimientos.Columns["Cantidad"]
                    .HeaderText = "Cantidad";

                dgvMovimientos.Columns["Cantidad"]
                    .DefaultCellStyle.Format = "N2";
            }

            if (dgvMovimientos.Columns["StockAnterior"] != null)
            {
                dgvMovimientos.Columns["StockAnterior"]
                    .HeaderText = "Stock anterior";

                dgvMovimientos.Columns["StockAnterior"]
                    .DefaultCellStyle.Format = "N2";
            }

            if (dgvMovimientos.Columns["StockNuevo"] != null)
            {
                dgvMovimientos.Columns["StockNuevo"]
                    .HeaderText = "Stock nuevo";

                dgvMovimientos.Columns["StockNuevo"]
                    .DefaultCellStyle.Format = "N2";
            }

            if (dgvMovimientos.Columns["Fecha"] != null)
            {
                dgvMovimientos.Columns["Fecha"]
                    .HeaderText = "Fecha";

                dgvMovimientos.Columns["Fecha"]
                    .DefaultCellStyle.Format =
                    "dd/MM/yyyy HH:mm";
            }

            if (dgvMovimientos.Columns["Concepto"] != null)
                dgvMovimientos.Columns["Concepto"]
                    .HeaderText = "Concepto";

            if (dgvMovimientos.Columns["Referencia"] != null)
                dgvMovimientos.Columns["Referencia"]
                    .HeaderText = "Referencia";
        }

        private void cmbProducto_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            if (cmbProducto.SelectedItem == null)
            {
                txtStockActual.Clear();
                ActualizarStockNuevo();
                return;
            }

            ProductoInventario producto =
                (ProductoInventario)
                    cmbProducto.SelectedItem;

            txtStockActual.Text =
                producto.Stock.ToString("0.00");

            ActualizarStockNuevo();
        }

        private void cmbTipo_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            ActualizarStockNuevo();
        }

        private void txtCantidad_TextChanged(
            object sender,
            EventArgs e)
        {
            ActualizarStockNuevo();
        }

        private void ActualizarStockNuevo()
        {
            if (cmbProducto.SelectedItem == null)
            {
                lblStockNuevo.Text =
                    "Stock nuevo: 0.00";

                return;
            }

            ProductoInventario producto =
                (ProductoInventario)
                    cmbProducto.SelectedItem;

            decimal cantidad;

            if (!decimal.TryParse(
                txtCantidad.Text,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out cantidad))
            {
                lblStockNuevo.Text =
                    $"Stock nuevo: {producto.Stock:N2}";

                return;
            }

            if (cantidad < 0)
                cantidad = 0;

            decimal stockNuevo =
                producto.Stock;

            string tipo =
                cmbTipo.SelectedItem?.ToString();

            if (tipo == "Entrada")
            {
                stockNuevo =
                    producto.Stock + cantidad;
            }
            else if (tipo == "Salida")
            {
                stockNuevo =
                    producto.Stock - cantidad;
            }
            else if (tipo == "Ajuste")
            {
                stockNuevo =
                    cantidad;
            }

            lblStockNuevo.Text =
                $"Stock nuevo: {stockNuevo:N2}";
        }

        private bool ObtenerCantidad(
            out decimal cantidad)
        {
            cantidad = 0;

            if (string.IsNullOrWhiteSpace(
                txtCantidad.Text))
            {
                MessageBox.Show(
                    "Debe introducir la cantidad.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtCantidad.Focus();
                return false;
            }

            if (!decimal.TryParse(
                txtCantidad.Text,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out cantidad))
            {
                MessageBox.Show(
                    "La cantidad no es válida.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtCantidad.Focus();
                return false;
            }

            if (cantidad <= 0)
            {
                MessageBox.Show(
                    "La cantidad debe ser mayor que cero.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtCantidad.Focus();
                return false;
            }

            return true;
        }

        private void RegistrarMovimiento()
        {
            try
            {
                if (cmbProducto.SelectedItem == null)
                {
                    MessageBox.Show(
                        "Debe seleccionar un producto.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    cmbProducto.Focus();
                    return;
                }

                if (cmbUsuario.SelectedItem == null)
                {
                    MessageBox.Show(
                        "Debe seleccionar un usuario.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    cmbUsuario.Focus();
                    return;
                }

                if (cmbTipo.SelectedItem == null)
                {
                    MessageBox.Show(
                        "Debe seleccionar el tipo de movimiento.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    cmbTipo.Focus();
                    return;
                }

                if (!ObtenerCantidad(
                    out decimal cantidad))
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(
                    txtConcepto.Text))
                {
                    MessageBox.Show(
                        "Debe indicar el concepto del movimiento.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtConcepto.Focus();
                    return;
                }

                ProductoInventario producto =
                    (ProductoInventario)
                        cmbProducto.SelectedItem;

                UsuarioInventarioItem usuario =
                    (UsuarioInventarioItem)
                        cmbUsuario.SelectedItem;

                string tipo =
                    cmbTipo.SelectedItem.ToString();

                decimal stockAnterior =
                    producto.Stock;

                decimal stockNuevo;

                if (tipo == "Entrada")
                {
                    stockNuevo =
                        stockAnterior + cantidad;
                }
                else if (tipo == "Salida")
                {
                    stockNuevo =
                        stockAnterior - cantidad;

                    if (stockNuevo < 0)
                    {
                        MessageBox.Show(
                            "No hay suficiente stock para realizar " +
                            "esta salida.\n\n" +
                            $"Stock disponible: {stockAnterior:N2}",
                            "Validación",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return;
                    }
                }
                else
                {
                    stockNuevo = cantidad;
                }

                MovimientoInventario movimiento =
                    new MovimientoInventario
                    {
                        ProductoId =
                            producto.Id,

                        UsuarioId =
                            usuario.Id,

                        Tipo =
                            tipo,

                        Cantidad =
                            cantidad,

                        StockAnterior =
                            stockAnterior,

                        StockNuevo =
                            stockNuevo,

                        Concepto =
                            txtConcepto.Text.Trim(),

                        Referencia =
                            string.IsNullOrWhiteSpace(
                                txtReferencia.Text)
                                ? null
                                : txtReferencia.Text.Trim()
                    };

                negocio.RegistrarMovimiento(
                    movimiento);

                MessageBox.Show(
                    "Movimiento registrado correctamente.\n\n" +
                    $"Stock anterior: {stockAnterior:N2}\n" +
                    $"Stock nuevo: {stockNuevo:N2}",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarProductos();
                CargarMovimientos();

                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo registrar el movimiento.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            cmbProducto.SelectedIndex = -1;

            cmbTipo.SelectedIndex = 0;

            txtCantidad.Clear();
            txtStockActual.Clear();
            txtConcepto.Clear();
            txtReferencia.Clear();

            lblStockNuevo.Text =
                "Stock nuevo: 0.00";

            cmbProducto.Focus();
        }

        private void btnRegistrar_Click(
            object sender,
            EventArgs e)
        {
            RegistrarMovimiento();
        }

        private void btnLimpiar_Click(
            object sender,
            EventArgs e)
        {
            LimpiarCampos();
        }
    }

    public class UsuarioInventarioItem
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }
}