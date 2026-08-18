using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EcoTech.Entidades;
using EcoTech.Negocio;

namespace EcoTech.Presentacion
{
    public partial class FrmCuentasContables : Form
    {
        private readonly N_CuentasContables negocio =
            new N_CuentasContables();

        private List<CuentaContable> cuentas =
            new List<CuentaContable>();

        private int cuentaSeleccionadaId = 0;

        public FrmCuentasContables()
        {
            InitializeComponent();

            ConfigurarFormulario();
            CargarTipos();
            CargarCuentas();

            btnGuardar.Click += btnGuardar_Click;
            btnActualizar.Click += btnActualizar_Click;
            btnDesactivar.Click += btnDesactivar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            dgvCuentas.CellClick += dgvCuentas_CellClick;
        }

        private void ConfigurarFormulario()
        {
            Text = "EcoTech - Cuentas Contables";
            StartPosition = FormStartPosition.CenterScreen;

            chkActivo.Checked = true;

            dgvCuentas.ReadOnly = true;
            dgvCuentas.AllowUserToAddRows = false;
            dgvCuentas.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvCuentas.MultiSelect = false;
            dgvCuentas.AutoGenerateColumns = true;
            dgvCuentas.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CargarTipos()
        {
            cmbTipo.Items.Clear();

            cmbTipo.Items.Add("Activo");
            cmbTipo.Items.Add("Pasivo");
            cmbTipo.Items.Add("Patrimonio");
            cmbTipo.Items.Add("Ingreso");
            cmbTipo.Items.Add("Gasto");

            cmbTipo.SelectedIndex = -1;
        }

        private void CargarCuentas()
        {
            try
            {
                cuentas = negocio.Listar();

                dgvCuentas.DataSource = null;
                dgvCuentas.DataSource = cuentas;

                ConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar las cuentas contables.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnas()
        {
            if (dgvCuentas.Columns["Id"] != null)
                dgvCuentas.Columns["Id"].HeaderText = "ID";

            if (dgvCuentas.Columns["Codigo"] != null)
                dgvCuentas.Columns["Codigo"].HeaderText = "Código";

            if (dgvCuentas.Columns["Nombre"] != null)
                dgvCuentas.Columns["Nombre"].HeaderText = "Nombre";

            if (dgvCuentas.Columns["Tipo"] != null)
                dgvCuentas.Columns["Tipo"].HeaderText = "Tipo";

            if (dgvCuentas.Columns["Descripcion"] != null)
                dgvCuentas.Columns["Descripcion"]
                    .HeaderText = "Descripción";

            if (dgvCuentas.Columns["Activo"] != null)
                dgvCuentas.Columns["Activo"].HeaderText = "Activo";
        }

        private CuentaContable ObtenerDatosFormulario()
        {
            return new CuentaContable
            {
                Id = cuentaSeleccionadaId,

                Codigo = txtCodigo.Text.Trim(),

                Nombre = txtNombre.Text.Trim(),

                Tipo = cmbTipo.SelectedItem == null
                    ? ""
                    : cmbTipo.SelectedItem.ToString(),

                Descripcion = txtDescripcion.Text.Trim(),

                Activo = chkActivo.Checked
            };
        }

        private void GuardarCuenta()
        {
            try
            {
                CuentaContable cuenta =
                    ObtenerDatosFormulario();

                negocio.Insertar(cuenta);

                MessageBox.Show(
                    "Cuenta contable guardada correctamente.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarCuentas();
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

        private void ActualizarCuenta()
        {
            try
            {
                if (cuentaSeleccionadaId <= 0)
                {
                    MessageBox.Show(
                        "Debe seleccionar una cuenta.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                CuentaContable cuenta =
                    ObtenerDatosFormulario();

                negocio.Editar(cuenta);

                MessageBox.Show(
                    "Cuenta contable actualizada correctamente.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarCuentas();
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

        private void DesactivarCuenta()
        {
            try
            {
                if (cuentaSeleccionadaId <= 0)
                {
                    MessageBox.Show(
                        "Debe seleccionar una cuenta.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                DialogResult resultado =
                    MessageBox.Show(
                        "¿Está seguro de desactivar esta cuenta?",
                        "Confirmar",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                if (resultado != DialogResult.Yes)
                    return;

                negocio.Desactivar(
                    cuentaSeleccionadaId);

                MessageBox.Show(
                    "Cuenta contable desactivada correctamente.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarCuentas();
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

        private void dgvCuentas_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dgvCuentas.Rows[e.RowIndex].DataBoundItem
                is CuentaContable cuenta)
            {
                cuentaSeleccionadaId = cuenta.Id;

                txtCodigo.Text =
                    cuenta.Codigo;

                txtNombre.Text =
                    cuenta.Nombre;

                cmbTipo.SelectedItem =
                    cuenta.Tipo;

                txtDescripcion.Text =
                    cuenta.Descripcion ?? "";

                chkActivo.Checked =
                    cuenta.Activo;
            }
        }

        private void LimpiarCampos()
        {
            cuentaSeleccionadaId = 0;

            txtCodigo.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();

            cmbTipo.SelectedIndex = -1;

            chkActivo.Checked = true;

            dgvCuentas.ClearSelection();

            txtCodigo.Focus();
        }

        private void btnGuardar_Click(
            object sender,
            EventArgs e)
        {
            GuardarCuenta();
        }

        private void btnActualizar_Click(
            object sender,
            EventArgs e)
        {
            ActualizarCuenta();
        }

        private void btnDesactivar_Click(
            object sender,
            EventArgs e)
        {
            DesactivarCuenta();
        }

        private void btnLimpiar_Click(
            object sender,
            EventArgs e)
        {
            LimpiarCampos();
        }
    }
}