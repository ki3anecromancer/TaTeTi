using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Necrolandia
{
    /// <summary>
    /// Lógica de interacción para TaTeTi.xaml
    /// </summary>
    public partial class TaTeTi : Window
    {
        bool turnoJug1;
        bool victoria = false;
        string nombre1, nombre2;
        int victorias1, victorias2;
        char piezaJug1;
        bool tornado;

        Button[] botones = new Button[9];
        Random randNum = new Random(); // Para numero aleatorio
        // Botones auxiliares para el modo tornado
        Button auxBoton1 = new Button();
        Button auxBoton2 = new Button();
        Button auxBoton3 = new Button();

        public TaTeTi()
        {
            InitializeComponent();

            // Agregar botones a la lista (botones) 
            botones[0] = B0_0;
            botones[1] = B0_1;
            botones[2] = B0_2;
            botones[3] = B1_0;
            botones[4] = B1_1;
            botones[5] = B1_2;
            botones[6] = B2_0;
            botones[7] = B2_1;
            botones[8] = B2_2;
        }

        // Verficar si hubo victoria
        private void Victoria()
        {
            // Verificar horizontal
            if ((B0_0.Content.ToString() != "") && (B0_0.Content == B0_1.Content) && (B0_1.Content == B0_2.Content)) victoria = true;
            else if ((B1_0.Content.ToString() != "") && (B1_0.Content == B1_1.Content) && (B1_1.Content == B1_2.Content)) victoria = true;
            else if ((B2_0.Content.ToString() != "") && (B2_0.Content == B2_1.Content) && (B2_1.Content == B2_2.Content)) victoria = true;

            // Verificar vertical
            else if ((B0_0.Content.ToString() != "") && (B0_0.Content == B1_0.Content) && (B1_0.Content == B2_0.Content)) victoria = true;
            else if ((B0_1.Content.ToString() != "") && (B0_1.Content == B1_1.Content) && (B1_1.Content == B2_1.Content)) victoria = true;
            else if ((B0_2.Content.ToString() != "") && (B0_2.Content == B1_2.Content) && (B1_2.Content == B2_2.Content)) victoria = true;

            // Verificar diagonal
            else if ((B0_0.Content.ToString() != "") && (B0_0.Content == B1_1.Content) && (B1_1.Content == B2_2.Content)) victoria = true;
            else if ((B0_2.Content.ToString() != "") && (B0_2.Content == B1_1.Content) && (B1_1.Content == B2_0.Content)) victoria = true;
        }

        // Verificar si hubo empate
        private bool Empate()
        {
            bool empate = true;

            // Verificar si todos los botones ya fueron utilizados
            foreach(Button boton in botones)
            {
                if (boton.IsEnabled == true) empate = false;
            }

            return empate;
        }

        // Turno de la máquina
        private void MaquinaCheckGanador()
        {
            bool encontrado = false; // Buscar victoria

            int recorrer = 0; // 0 = ganar , 1 = impedir derrota

            while(recorrer < 2 && encontrado == false)
            {
                // Revisar horizontal
                if (encontrado == false) BuscarVictoria(B0_0, B0_1, B0_2, ref encontrado, recorrer);
                if (encontrado == false) BuscarVictoria(B1_0, B1_1, B1_2, ref encontrado, recorrer);
                if (encontrado == false) BuscarVictoria(B2_0, B2_1, B2_2, ref encontrado, recorrer);

                // Revisar vertical
                if (encontrado == false) BuscarVictoria(B0_0, B1_0, B2_0, ref encontrado, recorrer);
                if (encontrado == false) BuscarVictoria(B0_1, B1_1, B2_1, ref encontrado, recorrer);
                if (encontrado == false) BuscarVictoria(B0_2, B1_2, B2_2, ref encontrado, recorrer);

                // Revisar diagonal
                if (encontrado == false) BuscarVictoria(B0_0, B1_1, B2_2, ref encontrado, recorrer);
                if (encontrado == false) BuscarVictoria(B0_2, B1_1, B2_0, ref encontrado, recorrer);

                if (encontrado == false) recorrer++;
            }

            // Si no se encuentra un movimiento, realizar movimiento al azar
            if (encontrado == false) MoverMaquina();
        }

        // Verificar boton
        private void BuscarVictoria(Button boton1, Button boton2, Button boton3, ref bool encontrado, int recorrido)
        {
            int contJ = 0; // Contar si gana el jugador
            int contM = 0; // Contar si gana la maquina
            int contDisp = 0; // Espacios disponibles

            string piezaJ = piezaJug1.ToString();
            string piezaM = piezaJ == "X" ? "O" : "X";

            if (boton1.Content.ToString() == piezaJ) contJ++;
            if (boton1.Content.ToString() == piezaM) contM++;
            if (boton1.Content.ToString() == "") contDisp++;

            if (boton2.Content.ToString() == piezaJ) contJ++;
            if (boton2.Content.ToString() == piezaM) contM++;
            if (boton2.Content.ToString() == "") contDisp++;

            if (boton3.Content.ToString() == piezaJ) contJ++;
            if (boton3.Content.ToString() == piezaM) contM++;
            if (boton3.Content.ToString() == "") contDisp++;

            bool victoriaM = false;
            bool victoriaJ = false;

            if (contM == 2 && contDisp == 1) victoriaM = true;
            if (contJ == 2 && contDisp == 1) victoriaJ = true;

            // Primer recorrido buscar si hay ganador
            if(recorrido == 0)
            {
                if (victoriaM)
                {
                    if (boton1.IsEnabled) ImpedirVictoria(boton1);
                    else if (boton2.IsEnabled) ImpedirVictoria(boton2);
                    else if (boton3.IsEnabled) ImpedirVictoria(boton3);
                    encontrado = true;
                }
            }            
            // Segundo recorrido buscar si se puede evitar derrota
            else if (victoriaJ)
            {
                if (boton1.IsEnabled) ImpedirVictoria(boton1);
                else if (boton2.IsEnabled) ImpedirVictoria(boton2);
                else if (boton3.IsEnabled) ImpedirVictoria(boton3);
                encontrado = true;
            }
        }

        private void ImpedirVictoria(Button boton)
        {
            string pieza = piezaJug1 == 'X' ? "O" : "X"; // pieza de la maquina
            boton.Content = pieza;
            boton.IsEnabled = false;
        }

        // Movimiento de la máquina
        private void MoverMaquina()
        {
            int pos = randNum.Next(9); // [0,8]
            string piezaMaq = piezaJug1 == 'X' ? "O" : "X";

            // Buscar una posicion disponible
            while(botones[pos].IsEnabled == false)
            {
                pos = randNum.Next(9);
            }

            // Cuando se encuentra la posición poner los valores
            botones[pos].Content = piezaMaq;
            botones[pos].IsEnabled = false;
        }

        private void CambiarTurno()
        {
            turnoJug1 = !turnoJug1;
            TurnoDe();
        }

        // Girar tablero de forma aleatoria -> Modo Tornado para 1 jugador
        private void ModoTornado()
        {
            // Numero que definirá qué volteo se hace            
            int volt = randNum.Next(1, 12); // [1,11]

            switch (volt)
            {
                case 1:
                    {
                        // Girar 90º a la derecha
                        GirarDerecha();
                    }
                    break;
                case 2:
                    {
                        // Girar 90º a la derecha + horinzontal
                        GirarDerecha();
                        GirarHorizontal();
                    }
                    break;
                case 3:
                    {
                        // Girar 90º a la derecha + vertical
                        GirarDerecha();
                        GirarVertical();
                    }
                    break;
                case 4:
                    {
                        // Girar 180º a la derecha
                        GirarDerecha();
                        GirarDerecha();
                    }
                    break;
                case 5:
                    {
                        // Girar 180º a la derecha + horinzontal
                        GirarDerecha();
                        GirarDerecha();
                        GirarHorizontal();
                    }
                    break;
                case 6:
                    {
                        // Girar 180º a la derecha + vertical
                        GirarDerecha();
                        GirarDerecha();
                        GirarVertical();
                    }
                    break;
                case 7:
                    {
                        // Girar 270º a la derecha
                        GirarDerecha();
                        GirarDerecha();
                    }
                    break;
                case 8:
                    {
                        // Girar 270º a la derecha + horinzontal
                        GirarDerecha();
                        GirarDerecha();
                        GirarDerecha();
                        GirarHorizontal();
                    }
                    break;
                case 9:
                    {
                        // Girar 270º a la derecha + vertical
                        GirarDerecha();
                        GirarDerecha();
                        GirarDerecha();
                        GirarVertical();
                    }
                    break;
                case 10:
                    {
                        GirarHorizontal();
                    }
                    break;
                case 11:
                    {
                        GirarVertical();
                    }
                    break;
            }
        }

        private void GirarDerecha()
        {
            IgualarBotonProp(auxBoton1, B0_0);
            IgualarBotonProp(auxBoton2, B0_1);
            IgualarBotonProp(auxBoton3, B0_2);

            IgualarBotonProp(B0_0, B2_0);
            IgualarBotonProp(B0_1, B1_0);
            IgualarBotonProp(B0_2, auxBoton1);

            IgualarBotonProp(B1_0, B2_1);
            IgualarBotonProp(B2_0, B2_2);

            IgualarBotonProp(B2_1, B1_2);

            IgualarBotonProp(B2_2, auxBoton3);
            IgualarBotonProp(B1_2, auxBoton2);
        }

        private void GirarVertical()
        {
            IgualarBotonProp(auxBoton1, B0_0);
            IgualarBotonProp(auxBoton2, B0_1);
            IgualarBotonProp(auxBoton3, B0_2);

            IgualarBotonProp(B0_0, B2_0);
            IgualarBotonProp(B0_1, B2_1);
            IgualarBotonProp(B0_2, B2_2);

            IgualarBotonProp(B2_0, auxBoton1);
            IgualarBotonProp(B2_1, auxBoton2);
            IgualarBotonProp(B2_2, auxBoton3);
        }

        private void GirarHorizontal()
        {
            IgualarBotonProp(auxBoton1, B0_0);
            IgualarBotonProp(auxBoton2, B1_0);
            IgualarBotonProp(auxBoton3, B2_0);

            IgualarBotonProp(B0_0, B0_2);
            IgualarBotonProp(B1_0, B1_2);
            IgualarBotonProp(B2_0, B2_2);

            IgualarBotonProp(B0_2, auxBoton1);
            IgualarBotonProp(B1_2, auxBoton2);
            IgualarBotonProp(B2_2, auxBoton3);
        }

        private void IgualarBotonProp(Button b1, Button b2)
        {
            b1.IsEnabled = b2.IsEnabled;
            b1.Content = b2.Content;
        }

        // Comenzar partida
        private void ComenzarPartida()
        {
            mostrarTurno();

            // Si solo hay un jugador y comienza la máquina
            if (CantJugadores1.IsChecked == true)
            {
                if (turnoJug1 == false) // Turno de la máquina
                {
                    MaquinaCheckGanador();

                    CambiarTurno();
                }
            }
        }

        private void mostrarTurno()
        {
            MostrarTurno.Visibility = Visibility.Visible;
            TurnoDe();
        }

        private void TurnoDe()
        {
            if (turnoJug1) TurnoNombre.Content = nombre1;
            else TurnoNombre.Content = nombre2;
        }

        private void MostrarPuntajes()
        {
            Jugador1.Text = nombre1;
            Puntaje1.Text = $"Victorias: {victorias1.ToString()}";

            Jugador2.Text = nombre2;
            Puntaje2.Text = $"Victorias: {victorias2.ToString()}";          
        }

        private void Boton_Continuar(object sender, RoutedEventArgs e)
        {
            if(CantJugadores1.IsChecked == true || CantJugadores2.IsChecked == true)
            {
                // Desactivar la elección de jugadores y el botón
                CantJugadores1.IsEnabled = false;
                CantJugadores2.IsEnabled = false;
                BotonContinuar.IsEnabled = false;

                // Un jugador
                if (CantJugadores1.IsChecked == true)
                {
                    NombrarJugador1.Visibility = Visibility.Visible;
                    UnJugadorOpciones.Visibility = Visibility.Visible;
                }

                // 2 jugadores
                if (CantJugadores2.IsChecked == true)
                {
                    NombrarJugador1.Visibility = Visibility.Visible;
                    NombrarJugador2.Visibility = Visibility.Visible;
                }
            }
            else
            {
                MessageBox.Show("Debes seleccionar al menos una opción", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Boton_Finalizar(object sender, RoutedEventArgs e)
        {
            if (CantJugadores2.IsChecked == true) // 2 jugadores
            {
                if (NombJugador1.Text == "" || NombJugador2.Text == "" || (piezaO.IsChecked == false && piezaX.IsChecked == false))
                {
                    MessageBox.Show("Debes completar todos los campos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    InicializarNuevaPartida();
                }
            } 

            if(CantJugadores1.IsChecked == true) // 1 jugador
            {
                if (NombJugador1.Text == "" || (piezaO.IsChecked == false && piezaX.IsChecked == false))
                {
                    MessageBox.Show("Debes completar todos los campos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    InicializarNuevaPartida();
                }
            }
        }

        private void InicializarNuevaPartida()
        {
            // Ocultar ventana del inicio
            VentanaComienzo.Visibility = Visibility.Hidden;

            // Implementar nombres
            nombre1 = NombJugador1.Text;

            if (CantJugadores2.IsChecked == true) nombre2 = NombJugador2.Text;
            else nombre2 = "Máquina";

            // Implementar pieza del jugador 1
            if (piezaO.IsChecked == true) piezaJug1 = 'O';
            else piezaJug1 = 'X';

            MostrarPuntajes();

            // Activar todos los botones y darles contenido -> content = ""
            foreach (Button boton in botones)
            {
                boton.IsEnabled = true;
                boton.Content = "";
            }

            // Si se activa tornado en un jugador
            if (ActivarModoTornado.IsChecked == true) tornado = true;

            // Elegir qué jugador comienza al azar
            ElegirPrimerJugador();

            // Comenzar la partida
            ComenzarPartida();
        }

        private void ElegirPrimerJugador()
        {
            int res = randNum.Next(1, 3); // [1,2]

            if (res == 1) turnoJug1 = true;
            else turnoJug1 = false;
        }

        private void Boton_Comenzar(object sender, RoutedEventArgs e)
        {
            ElegirPrimerJugador();

            // Activar todos los botones
            foreach (Button boton in botones)
            {
                boton.IsEnabled = true;
                boton.Content = "";
            }

            victoria = false; // Reiniciar victoria

            ComenzarPartida();
        }

        private void Boton_Reiniciar(object sender, RoutedEventArgs e)
        {
            // Reiniciar contador de victorias
            victorias1 = 0;
            victorias2 = 0;

            // Poner todos los botones en valor por defecto
            foreach (Button boton in botones)
            {
                boton.IsEnabled = true;
                boton.Content = "";
            }

            victoria = false;

            ElegirPrimerJugador();
            if (CantJugadores1.IsChecked == true) ComenzarPartida();
            
            MostrarPuntajes();
        }

        private void Boton_NuevaPartida(object sender, RoutedEventArgs e)
        {
            // Desactivar todos los botones
            foreach(Button boton in botones)
            {
                boton.IsEnabled = false;
            }

            // Desactivar los nombres
            Jugador1.Text = "";
            Jugador2.Text = "";
            Puntaje1.Text = "";
            Puntaje2.Text = "";

            // Reiniciar los puntajes
            victorias1 = 0;
            victorias2 = 0;

            // Reiniciar todos los valores
            CantJugadores1.IsChecked = false;
            CantJugadores2.IsChecked = false;
            CantJugadores1.IsEnabled = true;
            CantJugadores2.IsEnabled = true;
            BotonContinuar.IsEnabled = true;
            UnJugadorOpciones.Visibility = Visibility.Collapsed;
            NombrarJugador2.Visibility = Visibility.Hidden;
            NombrarJugador1.Visibility = Visibility.Hidden;
            NombJugador1.Text = "";
            NombJugador2.Text = "";
            piezaO.IsChecked = false;
            piezaX.IsChecked = false;
            MostrarTurno.Visibility = Visibility.Hidden;
            victoria = false;
            tornado = false;
            ActivarModoTornado.IsChecked = false;
            tornado = false;

            // Mostrar la ventana de inicio
            VentanaComienzo.Visibility = Visibility.Visible;
        }

        // Controlar todos los botones
        private void ElegirBoton(Button boton)
        {
            boton.IsEnabled = false;

            // [1 JUGADOR]
            if(CantJugadores1.IsChecked == true)
            {
                if (turnoJug1)
                {
                    // Mover jugador 1
                    if (piezaJug1 == 'X')
                    {
                        boton.Content = "X";
                    }
                    else
                    {
                        boton.Content = "O";
                    }

                    Victoria();

                    if (victoria)
                    {
                        victorias1++;
                        MessageBox.Show($"Ganó el jugador {nombre1}");
                        MostrarPuntajes();

                        foreach (Button b in botones)
                        {
                            b.IsEnabled = false;
                        }
                    }
                    else
                    {
                        if (Empate())
                        {
                            MessageBox.Show("Empate, ninguno ganó");
                        }
                        else
                        {
                            CambiarTurno();

                            MaquinaCheckGanador();

                            Victoria();

                            if (victoria)
                            {
                                victorias2++;
                                MessageBox.Show($"Ganó el jugador {nombre2}");
                                MostrarPuntajes();

                                foreach (Button b in botones)
                                {
                                    b.IsEnabled = false;
                                }
                            }
                            else
                            {
                                if (Empate())
                                {
                                    MessageBox.Show("Empate, ninguno ganó");
                                }
                                else
                                {
                                    if (tornado) ModoTornado();

                                    CambiarTurno();
                                }
                            }
                        }
                    }
                }
            }

            // Elegir que pieza utilizar [2 JUGADORES]
            if(CantJugadores2.IsChecked == true)
            {
                if (turnoJug1)
                {
                    if (piezaJug1 == 'X')
                    {
                        boton.Content = "X";
                    }
                    else
                    {
                        boton.Content = "O";
                    }

                    Victoria();

                    if (victoria)
                    {
                        victorias1++;
                        MessageBox.Show($"Ganó el jugador {nombre1}");
                    }
                }

                if (turnoJug1 == false)
                {
                    if (piezaJug1 == 'O')
                    {
                        boton.Content = "X";
                    }
                    else
                    {
                        boton.Content = "O";
                    }

                    Victoria();

                    if (victoria)
                    {
                        victorias2++;
                        MessageBox.Show($"Ganó el jugador {nombre2}");
                    }
                }

                MostrarPuntajes();

                if (victoria)
                {
                    foreach (Button b in botones)
                    {
                        b.IsEnabled = false;
                    }
                }
                else
                {
                    if (Empate())
                    {
                        MessageBox.Show("Empate, ninguno ganó");
                    }
                    else
                    {
                        CambiarTurno();
                    }
                }
            } 
        }

        private void BB0_0(object sender, RoutedEventArgs e)
        {
            ElegirBoton(B0_0);
        }

        private void BB0_1(object sender, RoutedEventArgs e)
        {
            ElegirBoton(B0_1);
        }

        private void BB0_2(object sender, RoutedEventArgs e)
        {
            ElegirBoton(B0_2);
        }

        private void BB1_0(object sender, RoutedEventArgs e)
        {
            ElegirBoton(B1_0);
        }

        private void BB1_1(object sender, RoutedEventArgs e)
        {
            ElegirBoton(B1_1);
        }

        private void BB1_2(object sender, RoutedEventArgs e)
        {
            ElegirBoton(B1_2);
        }

        private void BB2_0(object sender, RoutedEventArgs e)
        {
            ElegirBoton(B2_0);
        }

        private void BB2_1(object sender, RoutedEventArgs e)
        {
            ElegirBoton(B2_1);
        }

        private void BB2_2(object sender, RoutedEventArgs e)
        {
            ElegirBoton(B2_2);
        }
    }
}
