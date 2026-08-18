using System;
using System.Windows.Forms;
using EcoTech.Entidades;
using EcoTech.Negocio;
using EcoTech.Utilidades;

namespace EcoTech.Presentacion
{
    public partial class FrmUsuarios : Form
    {
        private readonly N_Usuarios negocio = new N_Usuarios();
        private int idUsuarioSeleccionado = 0;

        public FrmUsuarios()
        {
            InitializeComponent();

            ConfigurarFormulario();

            CargarRoles();

            CargarUsuarios();

            LimpiarCampos();

            btnGuardar.Click += btnGuardar_Click;
            btnActualizar.Click += btnActualizar_Click;
            btnDesactivar.Click += btnDesactivar_Click;
            btnLimpiar.Click += btnLimpiar_Click;

            dgvUsuarios.CellClick += dgvUsuarios_CellClick;

            txtBuscar.TextChanged += txtBuscar_TextChanged;
        }

        private void ConfigurarFormulario()
        {
            Text = "EcoTech - Gestión de Usuarios";
            StartPosition = FormStartPosition.CenterScreen;

            txtPassword.PasswordChar = '*';

            chkActivo.Checked = true;

            dgvUsuarios.AutoGenerateColumns = true;
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.AllowUserToAddRows = false;

            dgvUsuarios.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvUsuarios.MultiSelect = false;

            dgvUsuarios.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CargarUsuarios()
        {
            try
            {
                dgvUsuarios.DataSource = negocio.Listar();

                OcultarColumnasSensibles();
                ConfigurarColumnas();
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

        private void OcultarColumnasSensibles()
        {
            if (dgvUsuarios.Columns["PasswordHash"] != null)
                dgvUsuarios.Columns["PasswordHash"].Visible = false;

            if (dgvUsuarios.Columns["RolId"] != null)
                dgvUsuarios.Columns["RolId"].Visible = false;
        }
        private void ConfigurarColumnas()
        {
            if (dgvUsuarios.Columns["Id"] != null)
                dgvUsuarios.Columns["Id"].HeaderText = "ID";

            if (dgvUsuarios.Columns["NombreUsuario"] != null)
                dgvUsuarios.Columns["NombreUsuario"].HeaderText = "Usuario";

            if (dgvUsuarios.Columns["RolNombre"] != null)
                dgvUsuarios.Columns["RolNombre"].HeaderText = "Rol";

            if (dgvUsuarios.Columns["Activo"] != null)
                dgvUsuarios.Columns["Activo"].HeaderText = "Activo";

            if (dgvUsuarios.Columns["FechaCreacion"] != null)
                dgvUsuarios.Columns["FechaCreacion"].HeaderText =
                    "Fecha de creación";
        }

        private void BuscarUsuarios()
        {
            try
            {
                string texto = txtBuscar.Text.Trim();

                var usuarios = negocio.Listar();

                if (!string.IsNullOrWhiteSpace(texto))
                {
                    usuarios = usuarios.FindAll(u =>
                        u.NombreUsuario
                            .IndexOf(
                                texto,
                                StringComparison.OrdinalIgnoreCase) >= 0
                        ||
                        u.RolNombre
                            .IndexOf(
                                texto,
                                StringComparison.OrdinalIgnoreCase) >= 0);
                }

                dgvUsuarios.DataSource = usuarios;

                OcultarColumnasSensibles();
                ConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron buscar los usuarios.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CargarRoles()
        {
            try
            {
                // Los roles existentes en EcoTech:
                // 1 - Administrador
                // 2 - Vendedor
                // 3 - Contador

                cmbRol.Items.Clear();

                cmbRol.Items.Add(
                    new ComboBoxItem(1, "Administrador"));

                cmbRol.Items.Add(
                    new ComboBoxItem(2, "Vendedor"));

                cmbRol.Items.Add(
                    new ComboBoxItem(3, "Contador"));

                cmbRol.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar los roles.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void GuardarUsuario()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUsuario.Text))
                {
                    MessageBox.Show(
                        "Debe escribir un usuario.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtUsuario.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show(
                        "Debe escribir una contraseña.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtPassword.Focus();
                    return;
                }

                if (cmbRol.SelectedItem == null)
                {
                    MessageBox.Show(
                        "Debe seleccionar un rol.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    cmbRol.Focus();
                    return;
                }

                ComboBoxItem rol =
                    (ComboBoxItem)cmbRol.SelectedItem;

                Usuario usuario = new Usuario
                {
                    NombreUsuario = txtUsuario.Text.Trim(),

                    PasswordHash =
                        PasswordHelper.HashPassword(
                            txtPassword.Text),

                    RolId = rol.Id,

                    Activo = chkActivo.Checked
                };

                negocio.Insertar(usuario);

                MessageBox.Show(
                    "Usuario registrado correctamente.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarUsuarios();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo guardar el usuario.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ActualizarUsuario()
        {
            try
            {
                if (idUsuarioSeleccionado <= 0)
                {
                    MessageBox.Show(
                        "Seleccione un usuario.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                if (string.IsNullOrWhiteSpace(txtUsuario.Text))
                {
                    MessageBox.Show(
                        "Debe escribir un usuario.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtUsuario.Focus();
                    return;
                }

                if (cmbRol.SelectedItem == null)
                {
                    MessageBox.Show(
                        "Debe seleccionar un rol.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                ComboBoxItem rol =
                    (ComboBoxItem)cmbRol.SelectedItem;

                string passwordHash = string.Empty;

                if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    passwordHash =
                        PasswordHelper.HashPassword(
                            txtPassword.Text);
                }

                Usuario usuario = new Usuario
                {
                    Id = idUsuarioSeleccionado,

                    NombreUsuario =
                        txtUsuario.Text.Trim(),

                    PasswordHash = passwordHash,

                    RolId = rol.Id,

                    Activo = chkActivo.Checked
                };

                negocio.Editar(usuario);

                MessageBox.Show(
                    "Usuario actualizado correctamente.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarUsuarios();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo actualizar el usuario.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void DesactivarUsuario()
        {
            try
            {
                if (idUsuarioSeleccionado <= 0)
                {
                    MessageBox.Show(
                        "Seleccione un usuario.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                DialogResult resultado = MessageBox.Show(
                    "¿Está seguro de desactivar este usuario?",
                    "Confirmar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado != DialogResult.Yes)
                    return;

                negocio.Desactivar(
                    idUsuarioSeleccionado);

                MessageBox.Show(
                    "Usuario desactivado correctamente.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarUsuarios();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo desactivar el usuario.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            idUsuarioSeleccionado = 0;

            txtUsuario.Clear();
            txtPassword.Clear();

            cmbRol.SelectedIndex = -1;

            chkActivo.Checked = true;

            dgvUsuarios.ClearSelection();

            txtUsuario.Focus();
        }

        private void dgvUsuarios_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;

                DataGridViewRow fila =
                    dgvUsuarios.Rows[e.RowIndex];

                idUsuarioSeleccionado =
                    Convert.ToInt32(
                        fila.Cells["Id"].Value);

                txtUsuario.Text =
                    fila.Cells["NombreUsuario"]
                        .Value?.ToString() ?? "";

                int rolId =
                    Convert.ToInt32(
                        fila.Cells["RolId"].Value);

                for (int i = 0;
                     i < cmbRol.Items.Count;
                     i++)
                {
                    ComboBoxItem item =
                        (ComboBoxItem)cmbRol.Items[i];

                    if (item.Id == rolId)
                    {
                        cmbRol.SelectedIndex = i;
                        break;
                    }
                }

                chkActivo.Checked =
                    Convert.ToBoolean(
                        fila.Cells["Activo"].Value);

                txtPassword.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo seleccionar el usuario.\n\n" +
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
            BuscarUsuarios();
        }

        private void btnGuardar_Click(
            object sender,
            EventArgs e)
        {
            GuardarUsuario();
        }

        private void btnActualizar_Click(
            object sender,
            EventArgs e)
        {
            ActualizarUsuario();
        }

        private void btnDesactivar_Click(
            object sender,
            EventArgs e)
        {
            DesactivarUsuario();
        }

        private void btnLimpiar_Click(
            object sender,
            EventArgs e)
        {
            LimpiarCampos();
        }
    }

    public class ComboBoxItem
    {
        public int Id { get; }

        public string Nombre { get; }

        public ComboBoxItem(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}