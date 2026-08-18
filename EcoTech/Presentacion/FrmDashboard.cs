using System;
using System.Windows.Forms;
using EcoTech.Entidades;
using EcoTech.Negocio;
using System.Globalization;

namespace EcoTech.Presentacion
{
    public partial class FrmDashboard : Form
    {
        private readonly N_Dashboard negocio =
            new N_Dashboard();

        public FrmDashboard()
        {
            InitializeComponent();

            ConfigurarFormulario();
            CargarDashboard();

            btnActualizar.Click += btnActualizar_Click;
            btnCerrar.Click += btnCerrar_Click;
        }

        private void ConfigurarFormulario()
        {
            Text = "EcoTech - Dashboard";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;

            lblTitulo.Text =
                "Dashboard EcoTech";

            lblUsuarios.Text =
                "Usuarios";

            lblClientes.Text =
                "Clientes";

            lblProductos.Text =
                "Productos";

            lblCategorias.Text =
                "Categorías";

            lblProveedores.Text =
                "Proveedores";

            lblOrdenes.Text =
                "Órdenes de compra";

            lblRecepciones.Text =
                "Recepciones";

            lblCompras.Text =
                "Compras totales";

            lblCaja.Text =
                "Saldo de caja";
        }

        private void CargarDashboard()
        {
            try
            {
                Dashboard resumen =
                    negocio.ObtenerResumen();

                lblUsuariosValor.Text =
                    resumen.TotalUsuarios.ToString();

                lblClientesValor.Text =
                    resumen.TotalClientes.ToString();

                lblProductosValor.Text =
                    resumen.TotalProductos.ToString();

                lblCategoriasValor.Text =
                    resumen.TotalCategorias.ToString();

                lblProveedoresValor.Text =
                    resumen.TotalProveedores.ToString();

                lblOrdenesValor.Text =
                    resumen.TotalOrdenes.ToString();

                lblRecepcionesValor.Text =
                    resumen.TotalRecepciones.ToString();

                CultureInfo culturaRD =
    new CultureInfo("es-DO");

                lblComprasValor.Text =
                    resumen.ComprasTotales
                        .ToString("C2", culturaRD);

                lblCajaValor.Text =
                    resumen.SaldoCaja
                        .ToString("C2", culturaRD);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo cargar el Dashboard.\n\n" +
                    ex.Message,
                    "EcoTech",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click(
            object sender,
            EventArgs e)
        {
            CargarDashboard();
        }

        private void btnCerrar_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }
    }
}