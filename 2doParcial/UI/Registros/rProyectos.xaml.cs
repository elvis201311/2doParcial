using _2doParcial.BLL;
using _2doParcial.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _2doParcial.UI.Registros
{
    /// <summary>
    /// Interaction logic for rProyectos.xaml
    /// </summary>
    public partial class rProyectos : Window
    {
        private Proyectos proyectos = new Proyectos();
        public rProyectos()
        {
            InitializeComponent();
            this.DataContext = proyectos;

            TipoTareaComboBox.ItemsSource = TareasBLL.GetList();
            TipoTareaComboBox.SelectedValuePath = "TareaId";
            TipoTareaComboBox.DisplayMemberPath = "TipoTarea";
        }
        
        private void Cargar()
        {
            this.DataContext = null;
            this.DataContext = proyectos;
        }
      
        private void Limpiar()
        {
            this.proyectos = new Proyectos();
            this.DataContext = proyectos;
        }
       
        private bool Validar()
        {
            bool Validado = true;
            if (ProyectoIdTextbox.Text.Length == 0)
            {
                Validado = false;
                MessageBox.Show("Transaccion Fallida", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return Validado;
        }
       
        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            Proyectos encontrado = ProyectosBLL.Buscar(proyectos.ProyectoId);

            if (encontrado != null)
            {
                proyectos = encontrado;
                Cargar();
                //MessageBox.Show("Proyecto Encontrado", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Este Proyecto no fue encontrado.\n\nAsegurese que existe o cree uno nuevo.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                Limpiar();
                //—————————————————————————————————————[ Limpiar y hacer focus en el Id]—————————————————————————————————————
                ProyectoIdTextbox.Text = "";
                ProyectoIdTextbox.Focus();
            }
        }
        
        private void AgregarFilaButton_Click(object sender, RoutedEventArgs e)
        {
            var filaDetalle = new ProyectosDetalle
            {
                ProyectoId = this.proyectos.ProyectoId,
                TareaId = Convert.ToInt32(TipoTareaComboBox.SelectedValue.ToString()),
                //Tipo = ((Tareas)TipoTareaComboBox.SelectedItem),
                Requerimiento = (RequerimientoTextBox.Text),
                Tiempo = Convert.ToSingle(TiempoTextBox.Text)
            };
            //——————————————————————————————[Tiempo Total]——————————————————————————————
            proyectos.TiempoTotal += Convert.ToDouble(TiempoTextBox.Text.ToString());
            //——————————————————————————————————————————————————————————————————————————
            this.proyectos.Detalle.Add(filaDetalle);
            Cargar();

            TipoTareaComboBox.SelectedIndex = -1;
            RequerimientoTextBox.Clear();
            TiempoTextBox.Clear();
        }
        
        private void RemoverFilaButton_Click(object sender, RoutedEventArgs e)
        {
            if (DetalleDataGrid.Items.Count >= 1 && DetalleDataGrid.SelectedIndex <= DetalleDataGrid.Items.Count - 1)
            {
                proyectos.Detalle.RemoveAt(DetalleDataGrid.SelectedIndex);
                Cargar();
            }
        }
        
        private void NuevoButton_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }
        
        private void GuardarButton_Click(object sender, RoutedEventArgs e)
        {
            {
                if (!Validar())
                    return;

                if (ProyectoIdTextbox.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("El Campo (ProyectoId) esta vacio.\n\nDescriba el proyecto.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    ProyectoIdTextbox.Focus();
                    return;
                }

                if (DescripcionTextBox.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("El Campo (Descripcion del proyecto) esta vacio.\n\nDescriba el proyecto.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    DescripcionTextBox.Focus();
                    return;
                }

                var paso = ProyectosBLL.Guardar(this.proyectos);
                if (paso)
                {
                    Limpiar();
                    MessageBox.Show("Transaccion Exitosa", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show("Transaccion Fallida", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        
        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {
            {
                if (ProyectosBLL.Eliminar(Utilidades.ToInt(ProyectoIdTextbox.Text)))
                {
                    Limpiar();
                    MessageBox.Show("Registro Eliminado", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show("No se pudo eliminar el registro", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void TiempoTotalTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
       
        private void TiempoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (TiempoTextBox.Text.Trim() != "")
                {
                    double resultado = double.Parse(TiempoTextBox.Text);
                }
            }
            catch
            {
                MessageBox.Show($"El valor digitado en el campo (Tiempo) no es un numero.\n\nPorfavor, digite un numero.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
                TiempoTextBox.Clear();
                TiempoTextBox.Focus();
            }
        }
    }
}