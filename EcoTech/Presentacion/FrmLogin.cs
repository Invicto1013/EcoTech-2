using EcoTech.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoTech.Presentacion
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                string usuario = txtUsuario.Text.Trim();
                string password = txtPassword.Text;

                if (string.IsNullOrWhiteSpace(usuario))
                {
                    MessageBox.Show(
                        "Ingrese el usuario.",
                        "EcoTech",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtUsuario.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show(
                        "Ingrese la contraseña.",
                        "EcoTech",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtPassword.Focus();
                    return;
                }

                N_Usuarios negocio = new N_Usuarios();

                var usuarioAutenticado = negocio.Autenticar(usuario, password);

                if (usuarioAutenticado == null)
                {
                    MessageBox.Show(
                        "Usuario o contraseña incorrectos.",
                        "EcoTech",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    txtPassword.Clear();
                    txtPassword.Focus();
                    return;
                }

                MessageBox.Show(
    $"Bienvenido, {usuarioAutenticado.NombreUsuario}.\n" +
    $"Rol: {usuarioAutenticado.RolNombre}",
    "EcoTech",
    MessageBoxButtons.OK,
    MessageBoxIcon.Information);

                FrmPrincipal principal = new FrmPrincipal(usuarioAutenticado);

                Hide();

                principal.FormClosed += (s, args) => Close();

                principal.Show();

                // Más adelante aquí abriremos FrmPrincipal.
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo iniciar sesión.\n\nDetalle: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
