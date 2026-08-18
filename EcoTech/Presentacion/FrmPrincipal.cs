using System;
using System.Linq;
using System.Windows.Forms;
using EcoTech.Entidades;

namespace EcoTech.Presentacion
{
    public partial class FrmPrincipal : Form
    {
        private readonly Usuario usuarioActual;

        public FrmPrincipal(Usuario usuario)
        {
            InitializeComponent();

            usuarioActual = usuario;

            ConfigurarFormulario();
            AplicarPermisos();
        }

        private void ConfigurarFormulario()
        {
            Text = "EcoTech - Sistema de Gestión Empresarial";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
        }

        private void AplicarPermisos()
        {
            if (usuarioActual == null)
                return;

            string rol = usuarioActual.RolNombre?.Trim();

            if (string.IsNullOrWhiteSpace(rol))
                return;

            // Administrador tiene acceso completo.
            if (rol.Equals(
                "Administrador",
                StringComparison.OrdinalIgnoreCase))
            {
                MostrarMenu("Usuarios");
                MostrarMenu("Compras");
                MostrarMenu("Inventario");
                MostrarMenu("Contabilidad");
                MostrarMenu("Clientes");
                MostrarMenu("Reportes");

                return;
            }

            // Vendedor:
            // Puede trabajar con Clientes, Inventario y Reportes.
            if (rol.Equals(
                "Vendedor",
                StringComparison.OrdinalIgnoreCase))
            {
                OcultarMenu("Usuarios");
                OcultarMenu("Contabilidad");

                MostrarMenu("Compras");
                MostrarMenu("Inventario");
                MostrarMenu("Clientes");
                MostrarMenu("Reportes");

                return;
            }

            // Contador:
            // Puede trabajar con Contabilidad y Reportes.
            if (rol.Equals(
                "Contador",
                StringComparison.OrdinalIgnoreCase))
            {
                OcultarMenu("Usuarios");
                OcultarMenu("Compras");
                OcultarMenu("Inventario");
                OcultarMenu("Clientes");

                MostrarMenu("Contabilidad");
                MostrarMenu("Reportes");

                return;
            }
        }

        private void OcultarMenu(string texto)
        {
            ToolStripMenuItem menu =
                BuscarMenu(texto);

            if (menu != null)
                menu.Visible = false;
        }

        private void MostrarMenu(string texto)
        {
            ToolStripMenuItem menu =
                BuscarMenu(texto);

            if (menu != null)
                menu.Visible = true;
        }

        private ToolStripMenuItem BuscarMenu(string texto)
        {
            MenuStrip menuStrip =
                Controls
                    .OfType<MenuStrip>()
                    .FirstOrDefault();

            if (menuStrip == null)
                return null;

            foreach (ToolStripItem item in menuStrip.Items)
            {
                if (item is ToolStripMenuItem menu)
                {
                    ToolStripMenuItem encontrado =
                        BuscarMenuRecursivo(menu, texto);

                    if (encontrado != null)
                        return encontrado;
                }
            }

            return null;
        }

        private ToolStripMenuItem BuscarMenuRecursivo(
            ToolStripMenuItem menu,
            string texto)
        {
            if (menu.Text.Equals(
                texto,
                StringComparison.OrdinalIgnoreCase))
            {
                return menu;
            }

            foreach (ToolStripItem item in menu.DropDownItems)
            {
                if (item is ToolStripMenuItem subMenu)
                {
                    ToolStripMenuItem encontrado =
                        BuscarMenuRecursivo(subMenu, texto);

                    if (encontrado != null)
                        return encontrado;
                }
            }

            return null;
        }

        private void gestionarUsuariosToolStripMenuItem_Click(
            object sender,
            EventArgs e)
        {
            FrmUsuarios formulario =
                new FrmUsuarios();

            formulario.ShowDialog();
        }

        private void gestionarClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmClientes formulario =
                new FrmClientes();

            formulario.ShowDialog();
        }

        private void órdenesDeCompraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmOrdenesCompra formulario =
        new FrmOrdenesCompra(usuarioActual.Id);

            formulario.ShowDialog();
        }

        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmProductos formulario =
        new FrmProductos();

            formulario.ShowDialog();
        }

        private void movimientosDeInventarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmInventario formulario =
        new FrmInventario(usuarioActual.Id);

            formulario.ShowDialog();
        }

        private void cuentasContablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCuentasContables formulario =
        new FrmCuentasContables();

            formulario.ShowDialog();
        }

        private void asientosContablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (usuarioActual == null ||
        usuarioActual.Id <= 0)
            {
                MessageBox.Show(
                    "No se pudo identificar al usuario actual.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            FrmAsientosContables formulario =
                new FrmAsientosContables(usuarioActual.Id);

            formulario.ShowDialog();
        }

        private void cajaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (usuarioActual == null ||
        usuarioActual.Id <= 0)
            {
                MessageBox.Show(
                    "No se pudo identificar al usuario actual.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            FrmCaja formulario =
                new FrmCaja(usuarioActual.Id);

            formulario.ShowDialog();
        }

        private void categoríasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCategorias formulario =
        new FrmCategorias();

            formulario.ShowDialog();
        }

        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmProveedores formulario =
        new FrmProveedores();

            formulario.ShowDialog();
        }

        private void recepcionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (usuarioActual == null ||
        usuarioActual.Id <= 0)
            {
                MessageBox.Show(
                    "No se pudo identificar al usuario actual.",
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            FrmRecepcionesCompra formulario =
                new FrmRecepcionesCompra(usuarioActual.Id);

            formulario.ShowDialog();
        }

        private void dashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDashboard formulario =
       new FrmDashboard();

            formulario.ShowDialog();
        }
    }
}