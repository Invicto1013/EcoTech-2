using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using EcoTech.Entidades;
using EcoTech.Negocio;

namespace EcoTech.Presentacion
{
    public partial class FrmClientes : Form
    {
        private readonly N_Clientes negocio = new N_Clientes();
        private int idClienteSeleccionado = 0;

        public FrmClientes()
        {
            InitializeComponent();

            ConfigurarFormulario();
            CargarClientes();
            LimpiarCampos();

            btnGuardar.Click += btnGuardar_Click;
            btnActualizar.Click += btnActualizar_Click;
            btnDesactivar.Click += btnDesactivar_Click;
            btnLimpiar.Click += btnLimpiar_Click;

            dgvClientes.CellClick += dgvClientes_CellClick;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
        }

        private void ConfigurarFormulario()
        {
            Text = "EcoTech - Gestión de Clientes";
            StartPosition = FormStartPosition.CenterScreen;

            chkActivo.Checked = true;

            dgvClientes.AutoGenerateColumns = true;
            dgvClientes.ReadOnly = true;
            dgvClientes.AllowUserToAddRows = false;
            dgvClientes.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.MultiSelect = false;
            dgvClientes.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CargarClientes()
        {
            try
            {
                dgvClientes.DataSource = negocio.Listar();

                ConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar los clientes.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnas()
        {
            if (dgvClientes.Columns["Id"] != null)
                dgvClientes.Columns["Id"].HeaderText = "ID";

            if (dgvClientes.Columns["Nombre"] != null)
                dgvClientes.Columns["Nombre"].HeaderText = "Nombre";

            if (dgvClientes.Columns["Nit"] != null)
                dgvClientes.Columns["Nit"].HeaderText = "NIT";

            if (dgvClientes.Columns["Email"] != null)
                dgvClientes.Columns["Email"].HeaderText = "Email";

            if (dgvClientes.Columns["Telefono"] != null)
                dgvClientes.Columns["Telefono"].HeaderText = "Teléfono";

            if (dgvClientes.Columns["Direccion"] != null)
                dgvClientes.Columns["Direccion"].HeaderText = "Dirección";

            if (dgvClientes.Columns["LimiteCredito"] != null)
            {
                dgvClientes.Columns["LimiteCredito"].HeaderText =
                    "Límite de crédito";

                dgvClientes.Columns["LimiteCredito"].DefaultCellStyle
                    .Format = "N2";
            }

            if (dgvClientes.Columns["Activo"] != null)
                dgvClientes.Columns["Activo"].HeaderText = "Activo";

            if (dgvClientes.Columns["FechaRegistro"] != null)
            {
                dgvClientes.Columns["FechaRegistro"].HeaderText =
                    "Fecha de registro";

                dgvClientes.Columns["FechaRegistro"].DefaultCellStyle
                    .Format = "dd/MM/yyyy HH:mm";
            }
        }

        private void BuscarClientes()
        {
            try
            {
                string texto = txtBuscar.Text.Trim();

                List<Cliente> clientes = negocio.Listar();

                if (!string.IsNullOrWhiteSpace(texto))
                {
                    clientes = clientes.FindAll(cliente =>
                        cliente.Nombre.IndexOf(
                            texto,
                            StringComparison.OrdinalIgnoreCase) >= 0
                        ||
                        (!string.IsNullOrWhiteSpace(cliente.Nit) &&
                         cliente.Nit.IndexOf(
                             texto,
                             StringComparison.OrdinalIgnoreCase) >= 0)
                        ||
                        (!string.IsNullOrWhiteSpace(cliente.Email) &&
                         cliente.Email.IndexOf(
                             texto,
                             StringComparison.OrdinalIgnoreCase) >= 0)
                        ||
                        (!string.IsNullOrWhiteSpace(cliente.Telefono) &&
                         cliente.Telefono.IndexOf(
                             texto,
                             StringComparison.OrdinalIgnoreCase) >= 0));
                }

                dgvClientes.DataSource = clientes;

                ConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron buscar los clientes.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private bool ObtenerLimiteCredito(
            out decimal limiteCredito)
        {
            limiteCredito = 0;

            string texto =
                txtLimiteCredito.Text.Trim();

            if (string.IsNullOrWhiteSpace(texto))
            {
                MessageBox.Show(
                    "Debe indicar el límite de crédito.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtLimiteCredito.Focus();
                return false;
            }

            if (!decimal.TryParse(
                texto,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out limiteCredito))
            {
                MessageBox.Show(
                    "El límite de crédito no es válido.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtLimiteCredito.Focus();
                return false;
            }

            if (limiteCredito < 0)
            {
                MessageBox.Show(
                    "El límite de crédito no puede ser negativo.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtLimiteCredito.Focus();
                return false;
            }

            return true;
        }

        private void GuardarCliente()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show(
                        "Debe escribir el nombre del cliente.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtNombre.Focus();
                    return;
                }

                if (!ObtenerLimiteCredito(
                    out decimal limiteCredito))
                {
                    return;
                }

                Cliente cliente = new Cliente
                {
                    Nombre = txtNombre.Text.Trim(),
                    Nit = txtNit.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim(),
                    Direccion = txtDireccion.Text.Trim(),
                    LimiteCredito = limiteCredito,
                    Activo = chkActivo.Checked
                };

                negocio.Insertar(cliente);

                MessageBox.Show(
                    "Cliente registrado correctamente.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarClientes();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo guardar el cliente.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ActualizarCliente()
        {
            try
            {
                if (idClienteSeleccionado <= 0)
                {
                    MessageBox.Show(
                        "Seleccione un cliente.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show(
                        "Debe escribir el nombre del cliente.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtNombre.Focus();
                    return;
                }

                if (!ObtenerLimiteCredito(
                    out decimal limiteCredito))
                {
                    return;
                }

                Cliente cliente = new Cliente
                {
                    Id = idClienteSeleccionado,
                    Nombre = txtNombre.Text.Trim(),
                    Nit = txtNit.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim(),
                    Direccion = txtDireccion.Text.Trim(),
                    LimiteCredito = limiteCredito,
                    Activo = chkActivo.Checked
                };

                negocio.Editar(cliente);

                MessageBox.Show(
                    "Cliente actualizado correctamente.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarClientes();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo actualizar el cliente.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void DesactivarCliente()
        {
            try
            {
                if (idClienteSeleccionado <= 0)
                {
                    MessageBox.Show(
                        "Seleccione un cliente.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                DialogResult resultado = MessageBox.Show(
                    "¿Está seguro de desactivar este cliente?",
                    "Confirmar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado != DialogResult.Yes)
                    return;

                negocio.Desactivar(
                    idClienteSeleccionado);

                MessageBox.Show(
                    "Cliente desactivado correctamente.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarClientes();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo desactivar el cliente.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            idClienteSeleccionado = 0;

            txtNombre.Clear();
            txtNit.Clear();
            txtEmail.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
            txtLimiteCredito.Clear();

            chkActivo.Checked = true;

            dgvClientes.ClearSelection();

            txtNombre.Focus();
        }

        private void dgvClientes_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;

                DataGridViewRow fila =
                    dgvClientes.Rows[e.RowIndex];

                idClienteSeleccionado =
                    Convert.ToInt32(
                        fila.Cells["Id"].Value);

                txtNombre.Text =
                    fila.Cells["Nombre"].Value?.ToString()
                    ?? string.Empty;

                txtNit.Text =
                    fila.Cells["Nit"].Value?.ToString()
                    ?? string.Empty;

                txtEmail.Text =
                    fila.Cells["Email"].Value?.ToString()
                    ?? string.Empty;

                txtTelefono.Text =
                    fila.Cells["Telefono"].Value?.ToString()
                    ?? string.Empty;

                txtDireccion.Text =
                    fila.Cells["Direccion"].Value?.ToString()
                    ?? string.Empty;

                txtLimiteCredito.Text =
                    Convert.ToDecimal(
                        fila.Cells["LimiteCredito"].Value)
                    .ToString("0.00");

                chkActivo.Checked =
                    Convert.ToBoolean(
                        fila.Cells["Activo"].Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo seleccionar el cliente.\n\n" +
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
            BuscarClientes();
        }

        private void btnGuardar_Click(
            object sender,
            EventArgs e)
        {
            GuardarCliente();
        }

        private void btnActualizar_Click(
            object sender,
            EventArgs e)
        {
            ActualizarCliente();
        }

        private void btnDesactivar_Click(
            object sender,
            EventArgs e)
        {
            DesactivarCliente();
        }

        private void btnLimpiar_Click(
            object sender,
            EventArgs e)
        {
            LimpiarCampos();
        }
    }
}