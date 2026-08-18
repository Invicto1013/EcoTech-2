using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using EcoTech.Entidades;
using EcoTech.Negocio;

namespace EcoTech.Presentacion
{
    public partial class FrmProveedores : Form
    {
        private readonly N_Proveedores negocio =
            new N_Proveedores();

        private List<Proveedor> proveedores =
            new List<Proveedor>();

        private int proveedorSeleccionadoId = 0;

        public FrmProveedores()
        {
            InitializeComponent();

            ConfigurarFormulario();
            CargarProveedores();

            btnGuardar.Click += btnGuardar_Click;
            btnActualizar.Click += btnActualizar_Click;
            btnDesactivar.Click += btnDesactivar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            dgvProveedores.CellClick += dgvProveedores_CellClick;
        }

        private void ConfigurarFormulario()
        {
            Text = "EcoTech - Proveedores";
            StartPosition = FormStartPosition.CenterScreen;

            chkActivo.Checked = true;

            dgvProveedores.ReadOnly = true;
            dgvProveedores.AllowUserToAddRows = false;
            dgvProveedores.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvProveedores.MultiSelect = false;
            dgvProveedores.AutoGenerateColumns = true;
            dgvProveedores.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CargarProveedores()
        {
            try
            {
                proveedores = negocio.Listar();

                dgvProveedores.DataSource = null;
                dgvProveedores.DataSource = proveedores;

                ConfigurarColumnas();
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

        private void ConfigurarColumnas()
        {
            if (dgvProveedores.Columns["Id"] != null)
                dgvProveedores.Columns["Id"]
                    .HeaderText = "ID";

            if (dgvProveedores.Columns["Nombre"] != null)
                dgvProveedores.Columns["Nombre"]
                    .HeaderText = "Nombre";

            if (dgvProveedores.Columns["Nit"] != null)
                dgvProveedores.Columns["Nit"]
                    .HeaderText = "NIT";

            if (dgvProveedores.Columns["Email"] != null)
                dgvProveedores.Columns["Email"]
                    .HeaderText = "Email";

            if (dgvProveedores.Columns["Telefono"] != null)
                dgvProveedores.Columns["Telefono"]
                    .HeaderText = "Teléfono";

            if (dgvProveedores.Columns["Direccion"] != null)
                dgvProveedores.Columns["Direccion"]
                    .HeaderText = "Dirección";

            if (dgvProveedores.Columns["PlazoPago"] != null)
                dgvProveedores.Columns["PlazoPago"]
                    .HeaderText = "Plazo de pago";

            if (dgvProveedores.Columns["Activo"] != null)
                dgvProveedores.Columns["Activo"]
                    .HeaderText = "Activo";

            if (dgvProveedores.Columns["FechaRegistro"] != null)
            {
                dgvProveedores.Columns["FechaRegistro"]
                    .HeaderText = "Fecha registro";

                dgvProveedores.Columns["FechaRegistro"]
                    .DefaultCellStyle.Format =
                    "dd/MM/yyyy HH:mm";
            }
        }

        private bool ObtenerPlazoPago(
            out int plazoPago)
        {
            plazoPago = 0;

            if (string.IsNullOrWhiteSpace(
                txtPlazoPago.Text))
            {
                MessageBox.Show(
                    "Debe indicar el plazo de pago.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtPlazoPago.Focus();

                return false;
            }

            if (!int.TryParse(
                txtPlazoPago.Text.Trim(),
                NumberStyles.Integer,
                CultureInfo.CurrentCulture,
                out plazoPago))
            {
                MessageBox.Show(
                    "El plazo de pago debe ser un número entero.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtPlazoPago.Focus();

                return false;
            }

            if (plazoPago < 0)
            {
                MessageBox.Show(
                    "El plazo de pago no puede ser negativo.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtPlazoPago.Focus();

                return false;
            }

            return true;
        }

        private Proveedor ObtenerDatosFormulario()
        {
            ObtenerPlazoPago(
                out int plazoPago);

            return new Proveedor
            {
                Id = proveedorSeleccionadoId,

                Nombre =
                    txtNombre.Text.Trim(),

                Nit =
                    txtNit.Text.Trim(),

                Email =
                    txtEmail.Text.Trim(),

                Telefono =
                    txtTelefono.Text.Trim(),

                Direccion =
                    txtDireccion.Text.Trim(),

                PlazoPago =
                    plazoPago,

                Activo =
                    chkActivo.Checked
            };
        }

        private void GuardarProveedor()
        {
            try
            {
                if (!ObtenerPlazoPago(
                    out int plazoPago))
                {
                    return;
                }

                Proveedor proveedor =
                    new Proveedor
                    {
                        Nombre =
                            txtNombre.Text.Trim(),

                        Nit =
                            txtNit.Text.Trim(),

                        Email =
                            txtEmail.Text.Trim(),

                        Telefono =
                            txtTelefono.Text.Trim(),

                        Direccion =
                            txtDireccion.Text.Trim(),

                        PlazoPago =
                            plazoPago,

                        Activo =
                            chkActivo.Checked,

                        FechaRegistro =
                            DateTime.Now
                    };

                negocio.Insertar(proveedor);

                MessageBox.Show(
                    "Proveedor guardado correctamente.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarProveedores();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void ActualizarProveedor()
        {
            try
            {
                if (proveedorSeleccionadoId <= 0)
                {
                    MessageBox.Show(
                        "Debe seleccionar un proveedor.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                if (!ObtenerPlazoPago(
                    out int plazoPago))
                {
                    return;
                }

                Proveedor proveedor =
                    new Proveedor
                    {
                        Id =
                            proveedorSeleccionadoId,

                        Nombre =
                            txtNombre.Text.Trim(),

                        Nit =
                            txtNit.Text.Trim(),

                        Email =
                            txtEmail.Text.Trim(),

                        Telefono =
                            txtTelefono.Text.Trim(),

                        Direccion =
                            txtDireccion.Text.Trim(),

                        PlazoPago =
                            plazoPago,

                        Activo =
                            chkActivo.Checked
                    };

                negocio.Actualizar(proveedor);

                MessageBox.Show(
                    "Proveedor actualizado correctamente.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarProveedores();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void DesactivarProveedor()
        {
            try
            {
                if (proveedorSeleccionadoId <= 0)
                {
                    MessageBox.Show(
                        "Debe seleccionar un proveedor.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                DialogResult resultado =
                    MessageBox.Show(
                        "¿Está seguro de desactivar este proveedor?",
                        "Confirmar",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                if (resultado != DialogResult.Yes)
                    return;

                negocio.Desactivar(
                    proveedorSeleccionadoId);

                MessageBox.Show(
                    "Proveedor desactivado correctamente.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarProveedores();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dgvProveedores_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dgvProveedores.Rows[e.RowIndex].DataBoundItem
                is Proveedor proveedor)
            {
                proveedorSeleccionadoId =
                    proveedor.Id;

                txtNombre.Text =
                    proveedor.Nombre;

                txtNit.Text =
                    proveedor.Nit ?? "";

                txtEmail.Text =
                    proveedor.Email ?? "";

                txtTelefono.Text =
                    proveedor.Telefono ?? "";

                txtDireccion.Text =
                    proveedor.Direccion ?? "";

                txtPlazoPago.Text =
                    proveedor.PlazoPago.ToString();

                chkActivo.Checked =
                    proveedor.Activo;
            }
        }

        private void LimpiarCampos()
        {
            proveedorSeleccionadoId = 0;

            txtNombre.Clear();
            txtNit.Clear();
            txtEmail.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
            txtPlazoPago.Clear();

            chkActivo.Checked = true;

            dgvProveedores.ClearSelection();

            txtNombre.Focus();
        }

        private void btnGuardar_Click(
            object sender,
            EventArgs e)
        {
            GuardarProveedor();
        }

        private void btnActualizar_Click(
            object sender,
            EventArgs e)
        {
            ActualizarProveedor();
        }

        private void btnDesactivar_Click(
            object sender,
            EventArgs e)
        {
            DesactivarProveedor();
        }

        private void btnLimpiar_Click(
            object sender,
            EventArgs e)
        {
            LimpiarCampos();
        }
    }
}