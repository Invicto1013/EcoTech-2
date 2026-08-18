using EcoTech.Datos;
using EcoTech.Entidades;
using EcoTech.Negocio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace EcoTech.Presentacion
{
    public partial class FrmAsientosContables : Form
    {
        private readonly N_AsientosContables negocio =
            new N_AsientosContables();

        private readonly N_CuentasContables negocioCuentas =
            new N_CuentasContables();

        private List<CuentaContable> cuentas =
            new List<CuentaContable>();

        private int usuarioActualId = 0;

        public FrmAsientosContables(int usuarioId)
        {
            InitializeComponent();

            usuarioActualId = usuarioId;

            ConfigurarFormulario();
            CargarCuentas();
            CargarAsientos();

            btnRegistrar.Click += btnRegistrar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
        }

        private void ConfigurarFormulario()
        {
            Text = "EcoTech - Asientos Contables";
            StartPosition = FormStartPosition.CenterScreen;

            dtpFecha.Value = DateTime.Now;

            dgvAsientos.ReadOnly = true;
            dgvAsientos.AllowUserToAddRows = false;
            dgvAsientos.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvAsientos.MultiSelect = false;
            dgvAsientos.AutoGenerateColumns = true;
            dgvAsientos.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CargarCuentas()
        {
            try
            {
                cuentas = negocioCuentas.Listar();

                cmbCuenta.Items.Clear();

                foreach (CuentaContable cuenta in cuentas)
                {
                    if (cuenta.Activo)
                    {
                        cmbCuenta.Items.Add(cuenta);
                    }
                }

                cmbCuenta.SelectedIndex = -1;
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

        private void CargarAsientos()
        {
            try
            {
                dgvAsientos.DataSource = null;
                dgvAsientos.DataSource =
                    negocio.Listar();

                ConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar los asientos contables.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnas()
        {
            if (dgvAsientos.Columns["Id"] != null)
                dgvAsientos.Columns["Id"]
                    .HeaderText = "ID";

            if (dgvAsientos.Columns["Fecha"] != null)
            {
                dgvAsientos.Columns["Fecha"]
                    .HeaderText = "Fecha";

                dgvAsientos.Columns["Fecha"]
                    .DefaultCellStyle.Format =
                    "dd/MM/yyyy HH:mm";
            }

            if (dgvAsientos.Columns["CuentaCodigo"] != null)
                dgvAsientos.Columns["CuentaCodigo"]
                    .HeaderText = "Código";

            if (dgvAsientos.Columns["CuentaNombre"] != null)
                dgvAsientos.Columns["CuentaNombre"]
                    .HeaderText = "Cuenta";

            if (dgvAsientos.Columns["Concepto"] != null)
                dgvAsientos.Columns["Concepto"]
                    .HeaderText = "Concepto";

            if (dgvAsientos.Columns["Debe"] != null)
            {
                dgvAsientos.Columns["Debe"]
                    .HeaderText = "Debe";

                dgvAsientos.Columns["Debe"]
                    .DefaultCellStyle.Format = "N2";
            }

            if (dgvAsientos.Columns["Haber"] != null)
            {
                dgvAsientos.Columns["Haber"]
                    .HeaderText = "Haber";

                dgvAsientos.Columns["Haber"]
                    .DefaultCellStyle.Format = "N2";
            }

            if (dgvAsientos.Columns["UsuarioNombre"] != null)
                dgvAsientos.Columns["UsuarioNombre"]
                    .HeaderText = "Usuario";

            if (dgvAsientos.Columns["CuentaId"] != null)
                dgvAsientos.Columns["CuentaId"]
                    .Visible = false;

            if (dgvAsientos.Columns["UsuarioId"] != null)
                dgvAsientos.Columns["UsuarioId"]
                    .Visible = false;
        }

        private bool ObtenerMonto(
            string texto,
            string nombreCampo,
            out decimal monto)
        {
            monto = 0;

            if (string.IsNullOrWhiteSpace(texto))
            {
                monto = 0;
                return true;
            }

            if (!decimal.TryParse(
                texto,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out monto))
            {
                MessageBox.Show(
                    $"El valor de {nombreCampo} no es válido.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            if (monto < 0)
            {
                MessageBox.Show(
                    $"El valor de {nombreCampo} no puede ser negativo.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            return true;
        }

        private void RegistrarAsiento()
        {
            try
            {
                if (cmbCuenta.SelectedItem == null)
                {
                    MessageBox.Show(
                        "Debe seleccionar una cuenta contable.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    cmbCuenta.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(
                    txtConcepto.Text))
                {
                    MessageBox.Show(
                        "El concepto es obligatorio.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtConcepto.Focus();
                    return;
                }

                if (usuarioActualId <= 0)
                {
                    MessageBox.Show(
                        "El usuario actual no es válido.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                if (!ObtenerMonto(
                    txtDebe.Text,
                    "Debe",
                    out decimal debe))
                {
                    txtDebe.Focus();
                    return;
                }

                if (!ObtenerMonto(
                    txtHaber.Text,
                    "Haber",
                    out decimal haber))
                {
                    txtHaber.Focus();
                    return;
                }

                if (debe == 0 && haber == 0)
                {
                    MessageBox.Show(
                        "Debe registrar un valor en Debe o Haber.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                if (debe > 0 && haber > 0)
                {
                    MessageBox.Show(
                        "Un asiento no puede tener Debe y Haber " +
                        "al mismo tiempo.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                CuentaContable cuenta =
                    (CuentaContable)cmbCuenta.SelectedItem;

                AsientoContable asiento =
                    new AsientoContable
                    {
                        Fecha = dtpFecha.Value,

                        Concepto =
                            txtConcepto.Text.Trim(),

                        CuentaId =
                            cuenta.Id,

                        Debe =
                            debe,

                        Haber =
                            haber,

                        UsuarioId =
                            usuarioActualId
                    };

                negocio.Insertar(asiento);

                MessageBox.Show(
                    "Asiento contable registrado correctamente.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarAsientos();
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

        private void LimpiarCampos()
        {
            dtpFecha.Value = DateTime.Now;

            cmbCuenta.SelectedIndex = -1;

            txtConcepto.Clear();
            txtDebe.Clear();
            txtHaber.Clear();

            cmbCuenta.Focus();
        }

        private void btnRegistrar_Click(
            object sender,
            EventArgs e)
        {
            RegistrarAsiento();
        }

        private void btnLimpiar_Click(
            object sender,
            EventArgs e)
        {
            LimpiarCampos();
        }
    }
}