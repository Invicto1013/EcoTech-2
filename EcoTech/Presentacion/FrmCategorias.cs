using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EcoTech.Entidades;
using EcoTech.Negocio;

namespace EcoTech.Presentacion
{
    public partial class FrmCategorias : Form
    {
        private readonly N_Categorias negocio =
            new N_Categorias();

        private List<Categoria> categorias =
            new List<Categoria>();

        private int categoriaSeleccionadaId = 0;

        public FrmCategorias()
        {
            InitializeComponent();

            ConfigurarFormulario();
            CargarCategorias();

            btnGuardar.Click += btnGuardar_Click;
            btnActualizar.Click += btnActualizar_Click;
            btnDesactivar.Click += btnDesactivar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            dgvCategorias.CellClick += dgvCategorias_CellClick;
        }

        private void ConfigurarFormulario()
        {
            Text = "EcoTech - Categorías";
            StartPosition = FormStartPosition.CenterScreen;

            chkActivo.Checked = true;

            dgvCategorias.ReadOnly = true;
            dgvCategorias.AllowUserToAddRows = false;
            dgvCategorias.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvCategorias.MultiSelect = false;
            dgvCategorias.AutoGenerateColumns = true;
            dgvCategorias.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CargarCategorias()
        {
            try
            {
                categorias = negocio.Listar();

                dgvCategorias.DataSource = null;
                dgvCategorias.DataSource = categorias;

                ConfigurarColumnas();
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

        private void ConfigurarColumnas()
        {
            if (dgvCategorias.Columns["Id"] != null)
                dgvCategorias.Columns["Id"]
                    .HeaderText = "ID";

            if (dgvCategorias.Columns["Nombre"] != null)
                dgvCategorias.Columns["Nombre"]
                    .HeaderText = "Nombre";

            if (dgvCategorias.Columns["Descripcion"] != null)
                dgvCategorias.Columns["Descripcion"]
                    .HeaderText = "Descripción";

            if (dgvCategorias.Columns["Activo"] != null)
                dgvCategorias.Columns["Activo"]
                    .HeaderText = "Activo";
        }

        private Categoria ObtenerDatosFormulario()
        {
            return new Categoria
            {
                Id = categoriaSeleccionadaId,

                Nombre =
                    txtNombre.Text.Trim(),

                Descripcion =
                    txtDescripcion.Text.Trim(),

                Activo =
                    chkActivo.Checked
            };
        }

        private void GuardarCategoria()
        {
            try
            {
                Categoria categoria =
                    ObtenerDatosFormulario();

                negocio.Insertar(categoria);

                MessageBox.Show(
                    "Categoría guardada correctamente.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarCategorias();
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

        private void ActualizarCategoria()
        {
            try
            {
                if (categoriaSeleccionadaId <= 0)
                {
                    MessageBox.Show(
                        "Debe seleccionar una categoría.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                Categoria categoria =
                    ObtenerDatosFormulario();

                negocio.Actualizar(categoria);

                MessageBox.Show(
                    "Categoría actualizada correctamente.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarCategorias();
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

        private void DesactivarCategoria()
        {
            try
            {
                if (categoriaSeleccionadaId <= 0)
                {
                    MessageBox.Show(
                        "Debe seleccionar una categoría.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                DialogResult resultado =
                    MessageBox.Show(
                        "¿Está seguro de desactivar esta categoría?",
                        "Confirmar",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                if (resultado != DialogResult.Yes)
                    return;

                negocio.Desactivar(
                    categoriaSeleccionadaId);

                MessageBox.Show(
                    "Categoría desactivada correctamente.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarCategorias();
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

        private void dgvCategorias_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dgvCategorias.Rows[e.RowIndex].DataBoundItem
                is Categoria categoria)
            {
                categoriaSeleccionadaId =
                    categoria.Id;

                txtNombre.Text =
                    categoria.Nombre;

                txtDescripcion.Text =
                    categoria.Descripcion ?? "";

                chkActivo.Checked =
                    categoria.Activo;
            }
        }

        private void LimpiarCampos()
        {
            categoriaSeleccionadaId = 0;

            txtNombre.Clear();
            txtDescripcion.Clear();

            chkActivo.Checked = true;

            dgvCategorias.ClearSelection();

            txtNombre.Focus();
        }

        private void btnGuardar_Click(
            object sender,
            EventArgs e)
        {
            GuardarCategoria();
        }

        private void btnActualizar_Click(
            object sender,
            EventArgs e)
        {
            ActualizarCategoria();
        }

        private void btnDesactivar_Click(
            object sender,
            EventArgs e)
        {
            DesactivarCategoria();
        }

        private void btnLimpiar_Click(
            object sender,
            EventArgs e)
        {
            LimpiarCampos();
        }
    }
}