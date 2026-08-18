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
    public partial class FrmOrdenesCompra : Form
    {
        private readonly N_OrdenesCompra negocio =
            new N_OrdenesCompra();

        private readonly Conexion conexion =
            new Conexion();

        private readonly List<DetalleOrden> detalles =
            new List<DetalleOrden>();

        private int usuarioActualId = 0;

        public FrmOrdenesCompra(int usuarioId)
        {
            InitializeComponent();

            usuarioActualId = usuarioId;

            ConfigurarFormulario();
            CargarProveedores();
            CargarUsuarios();
            CargarProductos();
            CargarEstados();
            CargarOrdenes();

            LimpiarFormulario();

            btnAgregarDetalle.Click += btnAgregarDetalle_Click;
            btnEliminarDetalle.Click += btnEliminarDetalle_Click;
            btnGuardar.Click += btnGuardar_Click;
            btnLimpiar.Click += btnLimpiar_Click;

            dgvDetalles.CellClick += dgvDetalles_CellClick;
            dgvOrdenes.CellClick += dgvOrdenes_CellClick;
        }

        private void ConfigurarFormulario()
        {
            Text = "EcoTech - Órdenes de Compra";
            StartPosition = FormStartPosition.CenterScreen;

            dtpFecha.Value = DateTime.Now;
            dtpFechaEntrega.Value = DateTime.Now;

            dtpFechaEntrega.ShowCheckBox = true;
            dtpFechaEntrega.Checked = false;

            dgvDetalles.AutoGenerateColumns = true;
            dgvDetalles.ReadOnly = true;
            dgvDetalles.AllowUserToAddRows = false;
            dgvDetalles.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvDetalles.MultiSelect = false;
            dgvDetalles.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvOrdenes.AutoGenerateColumns = true;
            dgvOrdenes.ReadOnly = true;
            dgvOrdenes.AllowUserToAddRows = false;
            dgvOrdenes.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvOrdenes.MultiSelect = false;
            dgvOrdenes.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CargarProveedores()
        {
            try
            {
                cmbProveedor.Items.Clear();

                List<ProveedorItem> proveedores =
                    ObtenerProveedores();

                foreach (ProveedorItem proveedor in proveedores)
                {
                    cmbProveedor.Items.Add(proveedor);
                }

                cmbProveedor.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar los proveedores.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private List<ProveedorItem> ObtenerProveedores()
        {
            List<ProveedorItem> lista =
                new List<ProveedorItem>();

            using (var cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    SELECT
                        id,
                        nombre
                    FROM dbo.proveedores
                    WHERE activo = 1
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
                        lista.Add(
                            new ProveedorItem
                            {
                                Id = Convert.ToInt32(
                                    reader["id"]),

                                Nombre = reader["nombre"]
                                    .ToString()
                            });
                    }
                }
            }

            return lista;
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
                                new UsuarioItem
                                {
                                    Id = Convert.ToInt32(
                                        reader["id"]),

                                    Nombre = reader["usuario"]
                                        .ToString()
                                });
                        }
                    }
                }

                for (int i = 0;
                     i < cmbUsuario.Items.Count;
                     i++)
                {
                    UsuarioItem usuario =
                        (UsuarioItem)cmbUsuario.Items[i];

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

        private void CargarProductos()
        {
            try
            {
                cmbProducto.Items.Clear();

                using (var cn = conexion.ObtenerConexion())
                {
                    cn.Open();

                    string sql = @"
                        SELECT
                            id,
                            codigo,
                            nombre,
                            precio_costo
                        FROM dbo.productos
                        WHERE activo = 1
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
                            cmbProducto.Items.Add(
                                new ProductoItem
                                {
                                    Id = Convert.ToInt32(
                                        reader["id"]),

                                    Codigo = reader["codigo"]
                                        .ToString(),

                                    Nombre = reader["nombre"]
                                        .ToString(),

                                    PrecioCosto =
                                        Convert.ToDecimal(
                                            reader["precio_costo"])
                                });
                        }
                    }
                }

                cmbProducto.SelectedIndex = -1;
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

        private void CargarEstados()
        {
            cmbEstado.Items.Clear();

            cmbEstado.Items.Add("Pendiente");
            cmbEstado.Items.Add("Aprobada");
            cmbEstado.Items.Add("Recibida");
            cmbEstado.Items.Add("Cancelada");

            cmbEstado.SelectedItem = "Pendiente";
        }

        private void CargarOrdenes()
        {
            try
            {
                dgvOrdenes.DataSource =
                    negocio.Listar();

                ConfigurarColumnasOrdenes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar las órdenes.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnasOrdenes()
        {
            if (dgvOrdenes.Columns["Id"] != null)
                dgvOrdenes.Columns["Id"].HeaderText = "ID";

            if (dgvOrdenes.Columns["ProveedorId"] != null)
                dgvOrdenes.Columns["ProveedorId"].Visible = false;

            if (dgvOrdenes.Columns["UsuarioId"] != null)
                dgvOrdenes.Columns["UsuarioId"].Visible = false;

            if (dgvOrdenes.Columns["ProveedorNombre"] != null)
                dgvOrdenes.Columns["ProveedorNombre"]
                    .HeaderText = "Proveedor";

            if (dgvOrdenes.Columns["UsuarioNombre"] != null)
                dgvOrdenes.Columns["UsuarioNombre"]
                    .HeaderText = "Usuario";

            if (dgvOrdenes.Columns["Fecha"] != null)
            {
                dgvOrdenes.Columns["Fecha"]
                    .HeaderText = "Fecha";

                dgvOrdenes.Columns["Fecha"]
                    .DefaultCellStyle.Format =
                    "dd/MM/yyyy HH:mm";
            }

            if (dgvOrdenes.Columns["FechaEntrega"] != null)
            {
                dgvOrdenes.Columns["FechaEntrega"]
                    .HeaderText = "Fecha entrega";

                dgvOrdenes.Columns["FechaEntrega"]
                    .DefaultCellStyle.Format =
                    "dd/MM/yyyy";
            }

            if (dgvOrdenes.Columns["Subtotal"] != null)
            {
                dgvOrdenes.Columns["Subtotal"]
                    .HeaderText = "Subtotal";

                dgvOrdenes.Columns["Subtotal"]
                    .DefaultCellStyle.Format = "N2";
            }

            if (dgvOrdenes.Columns["Impuesto"] != null)
            {
                dgvOrdenes.Columns["Impuesto"]
                    .HeaderText = "Impuesto";

                dgvOrdenes.Columns["Impuesto"]
                    .DefaultCellStyle.Format = "N2";
            }

            if (dgvOrdenes.Columns["Total"] != null)
            {
                dgvOrdenes.Columns["Total"]
                    .HeaderText = "Total";

                dgvOrdenes.Columns["Total"]
                    .DefaultCellStyle.Format = "N2";
            }

            if (dgvOrdenes.Columns["Estado"] != null)
                dgvOrdenes.Columns["Estado"]
                    .HeaderText = "Estado";
        }

        private void AgregarDetalle()
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

                if (!ObtenerDecimal(
                    txtCantidad.Text,
                    "cantidad",
                    out decimal cantidad))
                {
                    return;
                }

                if (cantidad <= 0)
                {
                    MessageBox.Show(
                        "La cantidad debe ser mayor que cero.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                if (!ObtenerDecimal(
                    txtPrecio.Text,
                    "precio",
                    out decimal precio))
                {
                    return;
                }

                if (precio < 0)
                {
                    MessageBox.Show(
                        "El precio no puede ser negativo.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                ProductoItem producto =
                    (ProductoItem)cmbProducto.SelectedItem;

                decimal subtotal =
                    Math.Round(
                        cantidad * precio,
                        2);

                DetalleOrden detalle =
                    new DetalleOrden
                    {
                        ProductoId = producto.Id,
                        Cantidad = cantidad,
                        Precio = precio,
                        Subtotal = subtotal,
                        ProductoCodigo = producto.Codigo,
                        ProductoNombre = producto.Nombre
                    };

                detalles.Add(detalle);

                ActualizarTablaDetalles();
                CalcularTotales();

                cmbProducto.SelectedIndex = -1;
                txtCantidad.Clear();
                txtPrecio.Clear();

                cmbProducto.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo agregar el producto.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void EliminarDetalle()
        {
            try
            {
                if (dgvDetalles.CurrentRow == null)
                {
                    MessageBox.Show(
                        "Seleccione un producto de la orden.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                int indice =
                    dgvDetalles.CurrentRow.Index;

                if (indice < 0 ||
                    indice >= detalles.Count)
                {
                    return;
                }

                detalles.RemoveAt(indice);

                ActualizarTablaDetalles();
                CalcularTotales();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo eliminar el producto.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ActualizarTablaDetalles()
        {
            dgvDetalles.DataSource = null;

            dgvDetalles.DataSource =
                detalles.Select(d => new
                {
                    d.ProductoId,
                    Codigo = d.ProductoCodigo,
                    Producto = d.ProductoNombre,
                    d.Cantidad,
                    d.Precio,
                    d.Subtotal
                }).ToList();

            if (dgvDetalles.Columns["ProductoId"] != null)
                dgvDetalles.Columns["ProductoId"]
                    .Visible = false;

            if (dgvDetalles.Columns["Cantidad"] != null)
                dgvDetalles.Columns["Cantidad"]
                    .DefaultCellStyle.Format = "N2";

            if (dgvDetalles.Columns["Precio"] != null)
                dgvDetalles.Columns["Precio"]
                    .DefaultCellStyle.Format = "N2";

            if (dgvDetalles.Columns["Subtotal"] != null)
                dgvDetalles.Columns["Subtotal"]
                    .DefaultCellStyle.Format = "N2";
        }

        private void CalcularTotales()
        {
            decimal subtotal =
                detalles.Sum(d => d.Subtotal);

            decimal impuesto =
                Math.Round(
                    subtotal * 0.18m,
                    2);

            decimal total =
                subtotal + impuesto;

            lblSubtotal.Text =
                $"Subtotal: {subtotal:N2}";

            lblImpuesto.Text =
                $"Impuesto: {impuesto:N2}";

            lblTotal.Text =
                $"Total: {total:N2}";
        }

        private void GuardarOrden()
        {
            try
            {
                if (cmbProveedor.SelectedItem == null)
                {
                    MessageBox.Show(
                        "Debe seleccionar un proveedor.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    cmbProveedor.Focus();
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

                if (cmbEstado.SelectedItem == null)
                {
                    MessageBox.Show(
                        "Debe seleccionar un estado.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    cmbEstado.Focus();
                    return;
                }

                if (detalles.Count == 0)
                {
                    MessageBox.Show(
                        "La orden debe tener al menos un producto.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                ProveedorItem proveedor =
                    (ProveedorItem)cmbProveedor.SelectedItem;

                UsuarioItem usuario =
                    (UsuarioItem)cmbUsuario.SelectedItem;

                string estado =
                    cmbEstado.SelectedItem.ToString();

                decimal subtotal =
                    detalles.Sum(d => d.Subtotal);

                decimal impuesto =
                    Math.Round(
                        subtotal * 0.18m,
                        2);

                decimal total =
                    subtotal + impuesto;

                OrdenCompra orden =
                    new OrdenCompra
                    {
                        ProveedorId = proveedor.Id,
                        UsuarioId = usuario.Id,
                        Fecha = dtpFecha.Value,
                        FechaEntrega =
                            dtpFechaEntrega.Checked
                                ? dtpFechaEntrega.Value
                                : (DateTime?)null,
                        Subtotal = subtotal,
                        Impuesto = impuesto,
                        Total = total,
                        Estado = estado
                    };

                int ordenId =
                    negocio.Insertar(
                        orden,
                        detalles);

                MessageBox.Show(
                    $"Orden de compra #{ordenId} " +
                    "registrada correctamente.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarOrdenes();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo guardar la orden.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            detalles.Clear();

            ActualizarTablaDetalles();
            CalcularTotales();

            cmbProveedor.SelectedIndex = -1;

            CargarUsuarios();

            cmbProducto.SelectedIndex = -1;

            txtCantidad.Clear();
            txtPrecio.Clear();

            dtpFecha.Value = DateTime.Now;
            dtpFechaEntrega.Value = DateTime.Now;
            dtpFechaEntrega.Checked = false;

            cmbEstado.SelectedItem = "Pendiente";

            cmbProveedor.Focus();
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
                    $"Debe introducir la {nombreCampo}.",
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
                    $"La {nombreCampo} no es válida.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            return true;
        }

        private void dgvOrdenes_CellClick(
    object sender,
    DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;

                DataGridViewRow fila =
                    dgvOrdenes.Rows[e.RowIndex];

                int ordenId =
                    Convert.ToInt32(
                        fila.Cells["Id"].Value);

                List<DetalleOrden> detallesOrden =
                    negocio.ListarDetalles(ordenId);

                detalles.Clear();

                detalles.AddRange(detallesOrden);

                ActualizarTablaDetalles();

                CalcularTotales();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar los detalles de la orden.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void btnAgregarDetalle_Click(
            object sender,
            EventArgs e)
        {
            AgregarDetalle();
        }

        private void btnEliminarDetalle_Click(
            object sender,
            EventArgs e)
        {
            EliminarDetalle();
        }

        private void btnGuardar_Click(
            object sender,
            EventArgs e)
        {
            GuardarOrden();
        }

        private void btnLimpiar_Click(
            object sender,
            EventArgs e)
        {
            LimpiarFormulario();
        }

        private void dgvDetalles_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
        }
    }

    public class ProveedorItem
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }

    public class UsuarioItem
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }

    public class ProductoItem
    {
        public int Id { get; set; }

        public string Codigo { get; set; }

        public string Nombre { get; set; }

        public decimal PrecioCosto { get; set; }

        public override string ToString()
        {
            return $"{Codigo} - {Nombre}";
        }
    }
}