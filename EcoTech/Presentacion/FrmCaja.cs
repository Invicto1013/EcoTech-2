using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using EcoTech.Entidades;
using EcoTech.Negocio;

namespace EcoTech.Presentacion
{
    public partial class FrmCaja : Form
    {
        private readonly N_Caja negocio =
            new N_Caja();

        private readonly int usuarioActualId;

        private List<Caja> movimientos =
            new List<Caja>();

        public FrmCaja(int usuarioId)
        {
            InitializeComponent();

            usuarioActualId = usuarioId;

            ConfigurarFormulario();
            CargarTipos();
            CargarSaldo();
            CargarMovimientos();

            btnRegistrar.Click += btnRegistrar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
        }

        private void ConfigurarFormulario()
        {
            Text = "EcoTech - Caja";
            StartPosition = FormStartPosition.CenterScreen;

            lblSaldoValor.Text = "RD$ 0.00";

            dgvCaja.ReadOnly = true;
            dgvCaja.AllowUserToAddRows = false;
            dgvCaja.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvCaja.MultiSelect = false;
            dgvCaja.AutoGenerateColumns = true;
            dgvCaja.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CargarTipos()
        {
            cmbTipo.Items.Clear();

            cmbTipo.Items.Add("INGRESO");
            cmbTipo.Items.Add("EGRESO");

            cmbTipo.SelectedIndex = 0;
        }

        private void CargarSaldo()
        {
            try
            {
                decimal saldo =
                    negocio.ObtenerSaldoActual();

                lblSaldoValor.Text =
                    $"RD$ {saldo:N2}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo cargar el saldo de caja.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CargarMovimientos()
        {
            try
            {
                movimientos =
                    negocio.Listar();

                dgvCaja.DataSource = null;
                dgvCaja.DataSource = movimientos;

                ConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar los movimientos de caja.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnas()
        {
            if (dgvCaja.Columns["Id"] != null)
                dgvCaja.Columns["Id"]
                    .HeaderText = "ID";

            if (dgvCaja.Columns["UsuarioId"] != null)
                dgvCaja.Columns["UsuarioId"]
                    .Visible = false;

            if (dgvCaja.Columns["Fecha"] != null)
            {
                dgvCaja.Columns["Fecha"]
                    .HeaderText = "Fecha";

                dgvCaja.Columns["Fecha"]
                    .DefaultCellStyle.Format =
                    "dd/MM/yyyy HH:mm";
            }

            if (dgvCaja.Columns["Concepto"] != null)
                dgvCaja.Columns["Concepto"]
                    .HeaderText = "Concepto";

            if (dgvCaja.Columns["Ingreso"] != null)
            {
                dgvCaja.Columns["Ingreso"]
                    .HeaderText = "Ingreso";

                dgvCaja.Columns["Ingreso"]
                    .DefaultCellStyle.Format =
                    "N2";
            }

            if (dgvCaja.Columns["Egreso"] != null)
            {
                dgvCaja.Columns["Egreso"]
                    .HeaderText = "Egreso";

                dgvCaja.Columns["Egreso"]
                    .DefaultCellStyle.Format =
                    "N2";
            }

            if (dgvCaja.Columns["Saldo"] != null)
            {
                dgvCaja.Columns["Saldo"]
                    .HeaderText = "Saldo";

                dgvCaja.Columns["Saldo"]
                    .DefaultCellStyle.Format =
                    "N2";
            }

            if (dgvCaja.Columns["Tipo"] != null)
                dgvCaja.Columns["Tipo"]
                    .HeaderText = "Tipo";

            if (dgvCaja.Columns["UsuarioNombre"] != null)
                dgvCaja.Columns["UsuarioNombre"]
                    .HeaderText = "Usuario";
        }

        private bool ObtenerMonto(
            out decimal monto)
        {
            monto = 0;

            if (string.IsNullOrWhiteSpace(
                txtMonto.Text))
            {
                MessageBox.Show(
                    "Debe introducir el monto.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtMonto.Focus();

                return false;
            }

            if (!decimal.TryParse(
                txtMonto.Text,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out monto))
            {
                MessageBox.Show(
                    "El monto no es válido.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtMonto.Focus();

                return false;
            }

            if (monto <= 0)
            {
                MessageBox.Show(
                    "El monto debe ser mayor que cero.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtMonto.Focus();

                return false;
            }

            return true;
        }

        private void RegistrarMovimiento()
        {
            try
            {
                if (usuarioActualId <= 0)
                {
                    MessageBox.Show(
                        "El usuario actual no es válido.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

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

                if (string.IsNullOrWhiteSpace(
                    txtConcepto.Text))
                {
                    MessageBox.Show(
                        "Debe indicar el concepto.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtConcepto.Focus();

                    return;
                }

                if (!ObtenerMonto(
                    out decimal monto))
                {
                    return;
                }

                string tipo =
                    cmbTipo.SelectedItem.ToString();

                decimal ingreso = 0;
                decimal egreso = 0;

                if (tipo.Equals(
                    "INGRESO",
                    StringComparison.OrdinalIgnoreCase))
                {
                    ingreso = monto;
                }
                else
                {
                    egreso = monto;
                }

                Caja caja =
                    new Caja
                    {
                        UsuarioId =
                            usuarioActualId,

                        Fecha =
                            DateTime.Now,

                        Concepto =
                            txtConcepto.Text.Trim(),

                        Ingreso =
                            ingreso,

                        Egreso =
                            egreso,

                        Tipo =
                            tipo
                    };

                decimal saldoNuevo =
                    negocio.Registrar(caja);

                MessageBox.Show(
                    "Movimiento de caja registrado correctamente.\n\n" +
                    $"Tipo: {tipo}\n" +
                    $"Monto: RD$ {monto:N2}\n" +
                    $"Saldo nuevo: RD$ {saldoNuevo:N2}",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarSaldo();
                CargarMovimientos();

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
            cmbTipo.SelectedIndex = 0;

            txtConcepto.Clear();
            txtMonto.Clear();

            cmbTipo.Focus();
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
}