using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EcoTech.Entidades;
using EcoTech.Negocio;

namespace EcoTech.Presentacion
{
    public partial class FrmRecepcionesCompra : Form
    {
        private readonly N_RecepcionesCompra negocio =
            new N_RecepcionesCompra();

        private readonly int usuarioActualId;

        private List<RecepcionCompra> recepciones =
            new List<RecepcionCompra>();

        private List<RecepcionCompra> ordenesPendientes =
            new List<RecepcionCompra>();

        public FrmRecepcionesCompra(int usuarioId)
        {
            InitializeComponent();

            usuarioActualId = usuarioId;

            ConfigurarFormulario();
            CargarOrdenesPendientes();
            CargarRecepciones();

            btnRegistrar.Click += btnRegistrar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
        }

        private void ConfigurarFormulario()
        {
            Text = "EcoTech - Recepciones de Compra";
            StartPosition = FormStartPosition.CenterScreen;

            cmbOrden.DropDownStyle =
                ComboBoxStyle.DropDownList;

            dgvRecepciones.ReadOnly = true;
            dgvRecepciones.AllowUserToAddRows = false;
            dgvRecepciones.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvRecepciones.MultiSelect = false;
            dgvRecepciones.AutoGenerateColumns = true;
            dgvRecepciones.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CargarOrdenesPendientes()
        {
            try
            {
                ordenesPendientes =
                    negocio.ListarOrdenesPendientes();

                cmbOrden.DataSource = null;

                cmbOrden.DataSource =
                    ordenesPendientes;

                cmbOrden.DisplayMember =
                    "ProveedorNombre";

                cmbOrden.ValueMember =
                    "OrdenId";

                if (ordenesPendientes.Count > 0)
                {
                    cmbOrden.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar las órdenes pendientes.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CargarRecepciones()
        {
            try
            {
                recepciones =
                    negocio.Listar();

                dgvRecepciones.DataSource = null;
                dgvRecepciones.DataSource =
                    recepciones;

                ConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar las recepciones.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnas()
        {
            if (dgvRecepciones.Columns["Id"] != null)
                dgvRecepciones.Columns["Id"]
                    .HeaderText = "ID";

            if (dgvRecepciones.Columns["OrdenId"] != null)
                dgvRecepciones.Columns["OrdenId"]
                    .HeaderText = "Orden";

            if (dgvRecepciones.Columns["UsuarioId"] != null)
                dgvRecepciones.Columns["UsuarioId"]
                    .Visible = false;

            if (dgvRecepciones.Columns["Fecha"] != null)
            {
                dgvRecepciones.Columns["Fecha"]
                    .HeaderText = "Fecha";

                dgvRecepciones.Columns["Fecha"]
                    .DefaultCellStyle.Format =
                    "dd/MM/yyyy HH:mm";
            }

            if (dgvRecepciones.Columns["Observacion"] != null)
                dgvRecepciones.Columns["Observacion"]
                    .HeaderText = "Observación";

            if (dgvRecepciones.Columns["ProveedorNombre"] != null)
                dgvRecepciones.Columns["ProveedorNombre"]
                    .HeaderText = "Proveedor";

            if (dgvRecepciones.Columns["UsuarioNombre"] != null)
                dgvRecepciones.Columns["UsuarioNombre"]
                    .HeaderText = "Usuario";

            if (dgvRecepciones.Columns["OrdenTotal"] != null)
            {
                dgvRecepciones.Columns["OrdenTotal"]
                    .HeaderText = "Total orden";

                dgvRecepciones.Columns["OrdenTotal"]
                    .DefaultCellStyle.Format =
                    "N2";
            }

            if (dgvRecepciones.Columns["OrdenEstado"] != null)
                dgvRecepciones.Columns["OrdenEstado"]
                    .HeaderText = "Estado";
        }

        private void RegistrarRecepcion()
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

                if (cmbOrden.SelectedItem == null)
                {
                    MessageBox.Show(
                        "Debe seleccionar una orden de compra.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    cmbOrden.Focus();

                    return;
                }

                if (string.IsNullOrWhiteSpace(
                    txtObservacion.Text))
                {
                    MessageBox.Show(
                        "Debe indicar una observación.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtObservacion.Focus();

                    return;
                }

                RecepcionCompra ordenSeleccionada =
                    cmbOrden.SelectedItem
                    as RecepcionCompra;

                if (ordenSeleccionada == null)
                {
                    MessageBox.Show(
                        "La orden seleccionada no es válida.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                RecepcionCompra recepcion =
                    new RecepcionCompra
                    {
                        OrdenId =
                            ordenSeleccionada.OrdenId,

                        UsuarioId =
                            usuarioActualId,

                        Fecha =
                            DateTime.Now,

                        Observacion =
                            txtObservacion.Text.Trim()
                    };

                negocio.Registrar(recepcion);

                MessageBox.Show(
                    "Recepción registrada correctamente.\n\n" +
                    $"Orden: {recepcion.OrdenId}\n" +
                    $"Proveedor: " +
                    $"{ordenSeleccionada.ProveedorNombre}",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarOrdenesPendientes();
                CargarRecepciones();

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
            txtObservacion.Clear();

            if (cmbOrden.Items.Count > 0)
                cmbOrden.SelectedIndex = 0;

            cmbOrden.Focus();
        }

        private void btnRegistrar_Click(
            object sender,
            EventArgs e)
        {
            RegistrarRecepcion();
        }

        private void btnLimpiar_Click(
            object sender,
            EventArgs e)
        {
            LimpiarCampos();
        }
    }
}