using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Batalha_Naval
{
    class Program
    {
        #region Constantes
            const int TAM_TABULEIRO = 13;
        #endregion
        #region Enum
            enum TipoNavio
            {
                PortaAvioes,
                Encouracado,
                Cruzador,
                Destroyer,
                Submarino
            }
            enum TipoCelula
            {
                Agua,
                Erro,
                Acerto,
                PortaAvioes,
                Encouracado,
                Cruzador,
                Destroyer,
                Submarino
            }
        #endregion
        #region Structs
        public struct LinhaColorida
        {
                public string Texto;
                public ConsoleColor Cor;

                public LinhaColorida(string texto, ConsoleColor cor)
                {
                    Texto = texto;
                    Cor = cor;
                }
        }
        struct Coordenada
        {
            public int Linha;
            public int Coluna;

            public Coordenada(int linha, int coluna)
            {
                Linha = linha;
                Coluna = coluna;
            }
        }
        #endregion
        #region Estado do jogo
        static int scorePlayer1 = 0, scorePlayer2 = 0;
        static bool jogoAtivo = true;
        #endregion
        #region Utilidades
        static readonly Random random = new Random();
        #endregion
        #region Configuração
        static int corPlayer1, corPlayer2;
        static string nomePlayer1 = "", nomePlayer2 = "";
        static int quantPortaAvioes = 0; //1
        static int quantEncouracados = 0; //2
        static int quantCruzadores = 0; //2
        static int quantDestroyers = 0; //3
        static int quantSubmarinos = 1; //3
        #endregion
        #region Tabuleiros
        static TipoCelula[,] tabuleiroFrotaP1 = new TipoCelula[TAM_TABULEIRO, TAM_TABULEIRO];
        static TipoCelula[,] tabuleiroFrotaP2 = new TipoCelula[TAM_TABULEIRO, TAM_TABULEIRO];
        static TipoCelula[,] tabuleiroRadarP1 = new TipoCelula[TAM_TABULEIRO, TAM_TABULEIRO];
        static TipoCelula[,] tabuleiroRadarP2 = new TipoCelula[TAM_TABULEIRO, TAM_TABULEIRO];
        #endregion      

        #region Main
        static void Main(string[] args)
        {
            int opcao = 0;
            Coordenada posicaoTiro = new Coordenada();
            string posicionar = "N";

            string[] navio = new string[]
            {
                "                                    |__",
                "                                    |\\/",
                "                                    ---",
                "                                    / | [",
                "                             !      | |||",
                "                           _/|     _/|-++'",
                "                       +  +--|    |--|--|_ |-",
                "                    { /|__|  |/\\__|  |--- |||__/",
                "                   +---------------___[}-_===_.'____                 /\\",
                "               ____`-' ||___-{]_| _[}-  |     |_[___\\==--            \\/   _",
                " __..._____--==/___]_|__|_____________________________[___\\==--____,------' .7",
                "|                                                                     BB-61/",
                " \\_________________________________________________________________________|",
                "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
            };

            LinhaColorida[] menuArte = new LinhaColorida[]
            {
                new LinhaColorida("__________         __    __  .__         ", ConsoleColor.Cyan),
                new LinhaColorida("  _________.__    .__        ", ConsoleColor.Red),
                new LinhaColorida("\\______   \\_____ _/  |__/  |_|  |   ____ ", ConsoleColor.Cyan),
                new LinhaColorida(" /   _____/|  |__ |__|_____  ", ConsoleColor.Red),
                new LinhaColorida(" |    |  _/\\__  \\\\   __\\   __\\  | _/ __ \\", ConsoleColor.Cyan),
                new LinhaColorida(" \\_____  \\ |  |  \\|  \\  __ \\ ", ConsoleColor.Red),
                new LinhaColorida(" |    |   \\ / __ \\|  |  |  | |  |_\\  ___/", ConsoleColor.Cyan),
                new LinhaColorida(" /        \\|   Y  \\  |  |_| >", ConsoleColor.Red),
                new LinhaColorida(" |______  /(____  /__|  |__| |____/\\___ \\", ConsoleColor.Cyan),
                new LinhaColorida("/_______  /|___|  /__|   __/", ConsoleColor.Red),
                new LinhaColorida("        \\/      \\/                     \\/", ConsoleColor.Cyan),
                new LinhaColorida("        \\/      \\/   |__|   ", ConsoleColor.Red)
            };

            //inicializa os tabuleiros
            InicializarTabuleiro(tabuleiroFrotaP1);
            InicializarTabuleiro(tabuleiroFrotaP2);
            InicializarTabuleiro(tabuleiroRadarP1);
            InicializarTabuleiro(tabuleiroRadarP2);

            while (opcao != 1)
            {
                Console.Clear();
                // MENU
                ImprimirArte(menuArte);
                PularLinha(4);

                Console.WriteLine("[1] Começar novo jogo");
                Console.WriteLine("[2] Continuar");
                Console.WriteLine("[3] Instruções");
                Console.WriteLine("[4] Configurações");
                Console.WriteLine("[5] Sair");
                PularLinha(3);

                //imprime arte do navio
                foreach (string linha in navio)
                {
                    Console.WriteLine(linha);
                }
                PularLinha(2);

                // Lê a opção do usuário
                opcao = LerInteiro();
                Console.Clear();

                switch (opcao)
                {
                    //instruções do jogo
                    case 3:
                        string desejaconhecer;
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine("                        Instruções");
                        Console.ResetColor();

                        Console.WriteLine("O objetivo do jogo é destruir todos os navios do seu oponente.");
                        PularLinha();

                        Console.WriteLine("No início da partida, o campo inimigo estará totalmente oculto.");
                        Console.WriteLine("Você não saberá onde os navios do adversário estão posicionados.");
                        PularLinha();

                        Console.WriteLine("Para atacar, informe a LINHA e a COLUNA do local desejado.");
                        PularLinha();

                        Console.Write("Se você acertar um navio, a posição será marcada com ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(TipoCelula.Acerto);
                        Console.ResetColor();
                        Console.WriteLine(" e você poderá jogar novamente.");
                        PularLinha();

                        Console.Write("Se você errar o ataque, a posição será marcada com ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(TipoCelula.Erro);
                        Console.ResetColor();
                        Console.WriteLine(" e a vez passará para o seu adversário.");
                        PularLinha();

                        Console.WriteLine("O jogo termina quando um dos jogadores destruir todos os navios do oponente.");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        PularLinha();
                        Console.WriteLine("você deseja Conhecer a interface do jogo (S/N) ?");
                        Console.ResetColor();
                        desejaconhecer = (Console.ReadLine() ?? "").ToUpper();
                        Console.Clear();
                        if (desejaconhecer == "S")
                        {
                            tabuleiroRadarP1[0, 0] = TipoCelula.Erro;
                            tabuleiroRadarP1[7, 8] = TipoCelula.Erro;
                            tabuleiroRadarP1[3, 5] = TipoCelula.Erro;
                            tabuleiroRadarP1[9, 3] = TipoCelula.Erro;
                            tabuleiroRadarP1[10, 10] = TipoCelula.Acerto;
                            tabuleiroRadarP1[10, 11] = TipoCelula.Acerto;
                            tabuleiroRadarP1[10, 12] = TipoCelula.Acerto;
                            tabuleiroRadarP1[8, 7] = TipoCelula.Acerto;
                            tabuleiroFrotaP1[1, 1] = TipoCelula.Submarino;
                            tabuleiroFrotaP1[3, 9] = TipoCelula.Submarino;
                            tabuleiroFrotaP1[7, 8] = TipoCelula.PortaAvioes;
                            tabuleiroFrotaP1[7, 9] = TipoCelula.PortaAvioes;
                            tabuleiroFrotaP1[7, 10] = TipoCelula.Acerto;
                            tabuleiroFrotaP1[7, 11] = TipoCelula.PortaAvioes;
                            tabuleiroFrotaP1[7, 12] = TipoCelula.PortaAvioes;
                            tabuleiroFrotaP1[10, 4] = TipoCelula.Encouracado;
                            tabuleiroFrotaP1[11, 4] = TipoCelula.Encouracado;
                            tabuleiroFrotaP1[0, 5] = TipoCelula.Cruzador;
                            tabuleiroFrotaP1[1, 5] = TipoCelula.Cruzador;
                            tabuleiroFrotaP1[3, 12] = TipoCelula.Erro;
                            tabuleiroFrotaP1[4, 4] = TipoCelula.Erro;
                            tabuleiroFrotaP1[1, 9] = TipoCelula.Erro;
                            tabuleiroFrotaP1[2, 5] = TipoCelula.Cruzador;
                            tabuleiroFrotaP1[3, 5] = TipoCelula.Cruzador;
                            tabuleiroFrotaP1[11, 8] = TipoCelula.Acerto;
                            tabuleiroFrotaP1[11, 9] = TipoCelula.Acerto;
                            Console.Write("                  Player: Fábio");
                            Console.ResetColor();
                            Console.Write("    <---   jogador da vez");
                            PularLinha(2);
                            Console.ReadKey();
                            Console.WriteLine("Legenda: ");
                            PularLinha();
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(TipoCelula.Agua);
                            Console.ResetColor();
                            Console.WriteLine(" agua");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(TipoCelula.Erro);
                            Console.ResetColor();
                            Console.WriteLine(" errou");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(TipoCelula.Acerto);
                            Console.ResetColor();
                            Console.WriteLine(" acertou");
                            PularLinha();
                            Console.WriteLine("logo acima temos uma legenda auto explicativa que lhe informa o que cada simbolo significa");
                            PularLinha(2);
                            Console.ReadKey();
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("                   SUA FROTA");
                            Console.ResetColor();
                            PularLinha();
                            Console.Write("    1  2  3  4  5  6  7  8  9  10 11 12 13");
                            PularLinha();
                            ImprimeTabuleiro(tabuleiroFrotaP1);
                            PularLinha();
                            Console.WriteLine("Este é o campo onde você vizualiza suas embarcações e onde o inimigo lhe atacou");
                            Console.ReadKey();
                            PularLinha(2);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("                   SEU RADAR");
                            Console.ResetColor();
                            PularLinha();
                            
                            ImprimeTabuleiro(tabuleiroRadarP1);
                            PularLinha();
                            Console.WriteLine("Este é o campo onde você Realiza seus ataques");
                            Console.ReadKey();
                            PularLinha();
                            Console.WriteLine("insira a linha em que você quer atacar:");
                            Console.ReadKey();
                            Console.WriteLine("insira a coluna em que você quer atacar:");
                            Console.ReadKey();
                            PularLinha();
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("Presione qualquer tecla para voltar ao menu");
                            Console.ResetColor();
                            Console.ReadKey();
                            Console.Clear();
                            tabuleiroRadarP1[0, 0] = TipoCelula.Agua;
                            tabuleiroRadarP1[7, 8] = TipoCelula.Agua;
                            tabuleiroRadarP1[3, 5] = TipoCelula.Agua;
                            tabuleiroRadarP1[9, 3] = TipoCelula.Agua;
                            tabuleiroRadarP1[10, 10] = TipoCelula.Agua;
                            tabuleiroRadarP1[10, 11] = TipoCelula.Agua;
                            tabuleiroRadarP1[10, 12] = TipoCelula.Agua;
                            tabuleiroRadarP1[8, 7] = TipoCelula.Agua;
                            tabuleiroFrotaP1[1, 1] = TipoCelula.Agua;
                            tabuleiroFrotaP1[3, 9] = TipoCelula.Agua;
                            tabuleiroFrotaP1[7, 8] = TipoCelula.Agua;
                            tabuleiroFrotaP1[7, 9] = TipoCelula.Agua;
                            tabuleiroFrotaP1[7, 10] = TipoCelula.Agua;
                            tabuleiroFrotaP1[7, 11] = TipoCelula.Agua;
                            tabuleiroFrotaP1[7, 12] = TipoCelula.Agua;
                            tabuleiroFrotaP1[10, 4] = TipoCelula.Agua;
                            tabuleiroFrotaP1[11, 4] = TipoCelula.Agua;
                            tabuleiroFrotaP1[0, 5] = TipoCelula.Agua;
                            tabuleiroFrotaP1[1, 5] = TipoCelula.Agua;
                            tabuleiroFrotaP1[4, 4] = TipoCelula.Agua;
                            tabuleiroFrotaP1[1, 9] = TipoCelula.Agua;
                            tabuleiroFrotaP1[2, 5] = TipoCelula.Agua;
                            tabuleiroFrotaP1[3, 5] = TipoCelula.Agua;
                            tabuleiroFrotaP1[11, 8] = TipoCelula.Agua;
                            tabuleiroFrotaP1[11, 9] = TipoCelula.Agua;
                        }
                        break;
                    // configuração quantidade de navios
                    case 4:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("              configurações");
                        Console.ResetColor();
                        PularLinha(2);
                        Console.Write("Insira a quantidade de porta aviões(Tamanho 5) no jogo: ");
                        quantPortaAvioes = LerInteiro();
                        Console.Write("Insira a quantidade de encouraçados(Tamanho 4) no jogo: ");
                        quantEncouracados = LerInteiro();
                        Console.Write("Insira a quantidade de cruzadores(Tamanho 3) no jogo: ");
                        quantCruzadores = LerInteiro();
                        Console.Write("Insira a quantidade de destroyers(Tamanho 2) no jogo: ");
                        quantDestroyers = LerInteiro();
                        Console.Write("Insira a quantidade de submarinos(Tamanho 1) no jogo: ");
                        quantSubmarinos = LerInteiro();
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine("Retornando ao menu...");
                        Console.ResetColor();
                        Thread.Sleep(2000);
                        Console.Clear();
                        break;
                    //saida do jogo
                    case 5:
                        Console.WriteLine("Saindo do jogo...");
                        Thread.Sleep(1000); // Pequena pausa para o usuário ver a mensagem
                        Environment.Exit(0); // encerra o programa
                    break;
                }

            }
            // configurações iniciais do player 1
            CapturaNomeCor(1);
            PularLinha();
            Console.WriteLine("você deseja posicionar sua frota (S/N) ?");
            posicionar = (Console.ReadLine() ?? "").ToUpper();

            if (posicionar == "S")
            {
                PularLinha();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Pressione qualquer tecla para iniciar o posicionamento");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
                PosicionarFrotaManual(tabuleiroFrotaP1);
                Thread.Sleep(800);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("agora é a vez do segundo player");
                Console.ResetColor();
                Thread.Sleep(2000);
            }            

            else if (posicionar == "N")
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                PularLinha();
                Console.WriteLine("Enviando embarcações...");
                Console.ResetColor();
                Thread.Sleep(3000);
                //gera os barcos player 1
                GerarFrota(tabuleiroFrotaP1);
            }

            Console.Clear();
            // configurações iniciais do player 2
            CapturaNomeCor(2);
            PularLinha();
            Console.WriteLine("você deseja posicionar seus navios(S/N) ?");
            posicionar = (Console.ReadLine() ?? "").ToUpper();


            if (posicionar == "S")
            {
                PularLinha();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Pressione qualquer tecla para iniciar o posicionamento");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
                PosicionarFrotaManual(tabuleiroFrotaP2);
                Console.WriteLine("Barcos posicionados, a partida ira começar");
                Thread.Sleep(3000);
            }

            else if (posicionar == "N")
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                PularLinha();
                Console.WriteLine("Enviando embarcações...");
                Console.ResetColor();
                Thread.Sleep(3000);                

                // gera os barcos do player 2
                GerarFrota(tabuleiroFrotaP2);
            }

            if (opcao == 1)// opção jogar
            {
                // int pontosVitoria = CalcularPontosVitoria();
                while (jogoAtivo)
                {
                    Console.Clear();

                    //<------------PLAYER1------------->

                    //imprimindo a tela do player
                    ExibirTelaJogador(1);

                    // metodo dar tiro (coleta as cordenadas)
                    CapturarCoordenadaTiro(ref posicaoTiro, tabuleiroRadarP1);

                    // metodo que atualiza o tabuleiro após o tiro ser realizando
                    ProcessarTiro(tabuleiroFrotaP2, tabuleiroRadarP1, posicaoTiro);
                    
                    Console.Clear();

                    //<------------PLAYER2------------->

                    //imprime a tela do player 2
                    ExibirTelaJogador(2);

                    //metodo dar tiro (coleta as cordenadas)
                    CapturarCoordenadaTiro(ref posicaoTiro, tabuleiroRadarP2);

                    //metodo que atualiza o a tabuleiro após o tiro ser realizando
                    ProcessarTiro(tabuleiroFrotaP1, tabuleiroRadarP2, posicaoTiro);

                }
            }
        }
        #endregion

    //metodos
        #region Tabuleiro
        //gera os tabuleiros com "agua" em todas as posições
        static void InicializarTabuleiro(TipoCelula[,] tabuleiro)
        {
            for (int linha = 0; linha < tabuleiro.GetLength(0); linha++)
            {
                for (int coluna = 0; coluna < tabuleiro.GetLength(1); coluna++)
                {
                    tabuleiro[linha, coluna] = TipoCelula.Agua;
                }
            }
        }
        //verifica se o tiro acertou e altera os icones nos tabuleiros
        static void ProcessarTiro(TipoCelula[,] tabuleiroDefensor, TipoCelula[,] tabuleiroRadar, Coordenada posicaoTiro)
        {
            TipoCelula alvo = tabuleiroDefensor[posicaoTiro.Linha, posicaoTiro.Coluna];

            // ACERTO
            if (ContemNavio(alvo))
            {   
                // if(scorePlayer1 >= CalcularPontosVitoria() || scorePlayer2 >= CalcularPontosVitoria())
                // {
                //     jogoAtivo = false;
                //     Console.Clear();
                //     string vitorioso;
                //     vitorioso = scorePlayer1 == CalcularPontosVitoria() ? nomePlayer1 : nomePlayer2;
                //     Console.WriteLine(vitorioso + " venceu");
                // }
                tabuleiroDefensor[posicaoTiro.Linha, posicaoTiro.Coluna] = TipoCelula.Acerto;
                tabuleiroRadar[posicaoTiro.Linha, posicaoTiro.Coluna] = TipoCelula.Acerto;

                Console.Clear();

                if (tabuleiroDefensor == tabuleiroFrotaP2){
                    ExibirTelaJogador(1);
                    scorePlayer1++;
                }
                else if (tabuleiroDefensor == tabuleiroFrotaP1){
                    ExibirTelaJogador(2);
                    scorePlayer2++;
                }
                
                Console.ForegroundColor = ConsoleColor.Green;
                PularLinha();
                Console.WriteLine("\n💥 Você acertou um navio! Jogue novamente.");
                Console.ResetColor();

                Thread.Sleep(1500);

                CapturarCoordenadaTiro(ref posicaoTiro, tabuleiroRadar);
                ProcessarTiro(tabuleiroDefensor, tabuleiroRadar, posicaoTiro);
            }

            // ERRO
            else if (alvo == TipoCelula.Agua)
            {
                tabuleiroDefensor[posicaoTiro.Linha, posicaoTiro.Coluna] = TipoCelula.Erro;
                tabuleiroRadar[posicaoTiro.Linha, posicaoTiro.Coluna] = TipoCelula.Erro;

                Console.ForegroundColor = ConsoleColor.Yellow;
                PularLinha();
                Console.WriteLine("\n💧 Você errou o disparo! Vez do adversário.");
                Console.ResetColor();

                Thread.Sleep(2000);
            }
        }
        #endregion
        #region Navios
        static void GerarFrota(TipoCelula[,] tabuleiro)
        {
            GerarNavio(quantPortaAvioes, tabuleiro, TipoNavio.PortaAvioes);
            GerarNavio(quantEncouracados, tabuleiro, TipoNavio.Encouracado);
            GerarNavio(quantCruzadores, tabuleiro, TipoNavio.Cruzador);
            GerarNavio(quantDestroyers, tabuleiro, TipoNavio.Destroyer);
            GerarNavio(quantSubmarinos, tabuleiro, TipoNavio.Submarino);
        }
        static void GerarNavio(int quantidade, TipoCelula[,] tabuleiro, TipoNavio tipo)
        {
            TipoCelula tipoCelulaNavio = ConverterNavioParaCelula(tipo);
            int tamanho = ObterTamanhoNavio(tipo);

            for (int i = 0; i < quantidade; i++)
            {
                bool navioPosicionado = false;

                while (!navioPosicionado)
                {
                    int linha = random.Next(0, TAM_TABULEIRO);
                    int coluna = random.Next(0, TAM_TABULEIRO);
                    bool orientacaoHorizontal = random.Next(2) == 0;

                    navioPosicionado = PosicionarNavio(tabuleiro, linha, coluna, tamanho, tipoCelulaNavio, orientacaoHorizontal);
                }
            }
        }
        static bool PosicionarNavio(TipoCelula[,] tabuleiro, int linha, int coluna, int tamanho, TipoCelula barco, bool orientacaoHorizontal)
        {
            int tamanhoTabuleiro = tabuleiro.GetLength(0);

            // Verifica se o barco cabe no tabuleiro
            if (orientacaoHorizontal)
            {
                if (coluna + tamanho > tamanhoTabuleiro)
                    return false;
            }
            else
            {
                if (linha + tamanho > tamanhoTabuleiro)
                    return false;
            }

            // Verifica se já existe barco nas posições
            for (int i = 0; i < tamanho; i++)
            {
                int linhaAtual = orientacaoHorizontal ? linha : linha + i;
                int colunaAtual = orientacaoHorizontal ? coluna + i : coluna;

                if (tabuleiro[linhaAtual, colunaAtual] != TipoCelula.Agua)
                    return false;
            }

            // Posiciona o barco
            for (int i = 0; i < tamanho; i++)
            {
                int linhaAtual = orientacaoHorizontal ? linha : linha + i;
                int colunaAtual = orientacaoHorizontal ? coluna + i : coluna;

                tabuleiro[linhaAtual, colunaAtual] = barco;
            }

            return true;
        }
        static void PosicionarFrotaManual(TipoCelula[,] tabuleiro)
        {
            PosicionarNavioManual(quantPortaAvioes, TipoNavio.PortaAvioes, tabuleiro);
            PosicionarNavioManual(quantEncouracados, TipoNavio.Encouracado, tabuleiro);
            PosicionarNavioManual(quantCruzadores, TipoNavio.Cruzador, tabuleiro);
            PosicionarNavioManual(quantDestroyers, TipoNavio.Destroyer, tabuleiro);
            PosicionarNavioManual(quantSubmarinos, TipoNavio.Submarino, tabuleiro);
        }
        static void PosicionarNavioManual(int quantidade, TipoNavio tipo, TipoCelula[,] tabuleiro)
        {
            TipoCelula tipoCelulaNavio = ConverterNavioParaCelula(tipo);
            int tamanho = ObterTamanhoNavio(tipo);
            string nome = tipo.ToString();

            for (int i = 0; i < quantidade; i++)
            {
                bool navioPosicionado = false;

                while (!navioPosicionado)
                {
                    Console.Clear();
                    ExibirCabecalhoTabuleiro("frota");
                    ImprimeTabuleiro(tabuleiro);

                    Console.WriteLine($"Posicionando {nome} (tamanho {tamanho})");

                    Console.Write("Linha inicial: ");
                    int linha = LerInteiro() - 1;

                    Console.Write("Coluna inicial: ");
                    int coluna = LerInteiro() - 1;

                    bool orientacaoHorizontal = true;

                    if (tamanho > 1)
                    {
                        Console.Write("Horizontal? (s/n): ");
                        orientacaoHorizontal = (Console.ReadLine()?.Trim().ToLower() == "s");
                    }

                    navioPosicionado = PosicionarNavio(tabuleiro, linha, coluna, tamanho, tipoCelulaNavio, orientacaoHorizontal);

                    if (!navioPosicionado)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Não foi possível posicionar o barco.");
                        Console.ResetColor();
                        Console.ReadLine();
                    }
                }
            }
        }
        
        #endregion
        #region Mecânicas do jogo
        //coleta as coordenadas do ataque, linha e coluna
        static void CapturarCoordenadaTiro(ref Coordenada posicaoTiro, TipoCelula[,] tabuleiro)
        {
            PularLinha(2);
            Console.Write("insira a linha em que você quer atacar: ");
            posicaoTiro.Linha = LerInteiro() - 1;
            Console.Write("insira a coluna em que você quer atacar: ");
            posicaoTiro.Coluna = LerInteiro() - 1;

            posicaoTiro = ValidarCoordenadaTabuleiro(posicaoTiro, tabuleiro);
            posicaoTiro = ValidarTiroRepetido(posicaoTiro, tabuleiro);
        }
        //tratamento de erros (explosão da matriz)
        static Coordenada ValidarCoordenadaTabuleiro(Coordenada posicaoTiro, TipoCelula[,] tabuleiro)
        {
            while (posicaoTiro.Linha < 0 || posicaoTiro.Linha >= TAM_TABULEIRO ||
                posicaoTiro.Coluna < 0 || posicaoTiro.Coluna >= TAM_TABULEIRO)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("A posição está fora do radar, escolha outra.");
                Console.ResetColor();

                CapturarCoordenadaTiro(ref posicaoTiro, tabuleiro);
            }

            return posicaoTiro;
        }

        //tratamento de erros (repetição de jogada)
        static Coordenada ValidarTiroRepetido(Coordenada posicaoTiro, TipoCelula[,] tabuleiro)
        {
            while (tabuleiro[posicaoTiro.Linha, posicaoTiro.Coluna] != TipoCelula.Agua)
            {
                Console.WriteLine("Você já disparou neste quadrante, escolha outro.");

                CapturarCoordenadaTiro(ref posicaoTiro, tabuleiro);
            }

            return posicaoTiro;
        }
        #endregion
        #region Interface
        //imprime a asc art do navio 
        static void ImprimirArte(LinhaColorida[] arte)
        {
            for (int i = 0; i < arte.Length; i += 2)
            {
                // imprime a primeira linha do par
                Console.ForegroundColor = arte[i].Cor;
                Console.Write(arte[i].Texto);
                Console.ResetColor();

                // imprime a segunda linha do par, se existir
                if (i + 1 < arte.Length)
                {
                    Console.ForegroundColor = arte[i + 1].Cor;
                    Console.Write(arte[i + 1].Texto);
                    Console.ResetColor();
                }

                // quebra de linha após o par
                PularLinha();
            }
        }
        static void CapturaNomeCor(int num)
        {
            Console.WriteLine("player " + num);
            PularLinha();

            Console.Write("Insira o seu nome: ");
            string nomeDigitado = Console.ReadLine() ?? "";

            PularLinha();
            Console.WriteLine("Escolha uma cor: ");
            PularLinha();

            Console.Write("1 - ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(nomeDigitado);
            Console.ResetColor();

            Console.Write("             4 - ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(nomeDigitado);
            Console.ResetColor();

            Console.Write("             7 - ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(nomeDigitado);
            Console.ResetColor();

            PularLinha();

            Console.Write("2 - ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(nomeDigitado);
            Console.ResetColor();

            Console.Write("             5 - ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(nomeDigitado);
            Console.ResetColor();

            Console.Write("             8 - ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(nomeDigitado);
            Console.ResetColor();

            PularLinha();

            Console.Write("3 - ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(nomeDigitado);
            Console.ResetColor();

            Console.Write("             6 - ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(nomeDigitado);
            Console.ResetColor();

            Console.Write("             9 - ");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(nomeDigitado);
            Console.ResetColor();

            PularLinha();

            int corEscolhida; 
            corEscolhida = LerInteiro();
            
            if (num == 1)
            {
                nomePlayer1 = nomeDigitado;
                corPlayer1 = corEscolhida;
            }
            else
            {
                nomePlayer2 = nomeDigitado;
                corPlayer2 = corEscolhida;
            }
        }        
        //metodo que reune a chamada de um grupo de metodos que constituem a tela do player 
        static void ExibirTelaJogador(int jogadorAtual)
        {
            if (jogadorAtual == 1)
            {
                ExibirLegendaJogador(1);
                ExibirCabecalhoTabuleiro("frota");
                ImprimeTabuleiro(tabuleiroFrotaP1);
                ExibirCabecalhoTabuleiro("radar");
                ImprimeTabuleiro(tabuleiroRadarP1);
                PularLinha();
            }
            else if (jogadorAtual == 2)
            {
                ExibirLegendaJogador(2);
                ExibirCabecalhoTabuleiro("frota");
                ImprimeTabuleiro(tabuleiroFrotaP2);
                ExibirCabecalhoTabuleiro("radar");
                ImprimeTabuleiro(tabuleiroRadarP2);
                PularLinha();
            }
        }
        //exibe nome do player da vez e a legenda dos tabuleiros
        static void ExibirLegendaJogador(int idJogador)
        {
            Console.Write("                  Player: ");
            if (idJogador == 1)
            {
                Console.ForegroundColor = SelecaoCorPlayer(corPlayer1);  
                Console.Write(nomePlayer1);
            }
            else if (idJogador == 2)
            {
                Console.ForegroundColor = SelecaoCorPlayer(corPlayer2);                
                Console.Write(nomePlayer2);
            }
            Console.ResetColor();
            PularLinha(2);
            Console.WriteLine("Legenda: ");
            PularLinha();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(ObterSimboloCelula(TipoCelula.Agua));
            Console.ResetColor();
            Console.WriteLine(" agua");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(ObterSimboloCelula(TipoCelula.Erro));
            Console.ResetColor();
            Console.WriteLine(" errou");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(ObterSimboloCelula(TipoCelula.Acerto));
            Console.ResetColor();
            Console.WriteLine(" acertou");
            PularLinha(2);
        }
        //cabeçalho dos tabuleiros
        static void ExibirCabecalhoTabuleiro(string frotaOuRadar)
        {
            if (frotaOuRadar == "frota")
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("                   SUA FROTA");
            }
            else if (frotaOuRadar == "radar")
            {
                PularLinha(2);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("                   SEU RADAR");
            }
            Console.ResetColor();
            PularLinha();
            Console.Write("    1  2  3  4  5  6  7  8  9  10 11 12 13");
            PularLinha();

        }
        static void ImprimeTabuleiro(TipoCelula[,] tabuleiro)
        {
            for (int linha = 0; linha < tabuleiro.GetLength(0); linha++)
            {
                Console.Write((linha + 1).ToString("00"));
                for (int coluna = 0; coluna < tabuleiro.GetLength(1); coluna++)
                {
                    if (tabuleiro[linha, coluna] == TipoCelula.Agua)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    else if (tabuleiro[linha, coluna] == TipoCelula.Acerto)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (tabuleiro[linha, coluna] == TipoCelula.Erro)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    Console.Write("  " + ObterSimboloCelula(tabuleiro[linha, coluna]));
                    Console.ResetColor();
                }
                PularLinha();
            }
        }
        #endregion
        #region Auxiliares
        //valida e captura o numero digitado
        static int LerInteiro()
        {
            int numero;

            while (!int.TryParse(Console.ReadLine(), out numero))
            {
                Console.WriteLine("Digite um número válido:");
            }

            return numero;
        }
        //insere os espaços na interface 
        static void PularLinha() 
        {
            Console.WriteLine();
        }
        static void PularLinha(int linhas) 
        {
            for (int i = 0; i < linhas; i++)
                Console.WriteLine();
        }
        //retorna a cor do console
        static ConsoleColor SelecaoCorPlayer(int cor)
        {
            return cor switch
            {
                1 => ConsoleColor.Red,
                2 => ConsoleColor.Yellow,
                3 => ConsoleColor.Green,
                4 => ConsoleColor.Cyan,
                5 => ConsoleColor.DarkGreen,
                6 => ConsoleColor.Gray,
                7 => ConsoleColor.Blue,
                8 => ConsoleColor.DarkGray,
                9 => ConsoleColor.DarkMagenta,
                _ => ConsoleColor.White
            };
        }
        //axuliares do enum TipoNavio
        static TipoCelula ConverterNavioParaCelula(TipoNavio tipo)
        {
            return tipo switch
            {
                TipoNavio.Submarino => TipoCelula.Submarino,
                TipoNavio.PortaAvioes => TipoCelula.PortaAvioes,
                TipoNavio.Encouracado => TipoCelula.Encouracado,
                TipoNavio.Destroyer => TipoCelula.Destroyer,
                TipoNavio.Cruzador => TipoCelula.Cruzador,
                _ => TipoCelula.Agua
            };
        }
        static int ObterTamanhoNavio(TipoNavio tipo)
        {
            return tipo switch
            {
                TipoNavio.PortaAvioes => 5,
                TipoNavio.Encouracado => 4,
                TipoNavio.Cruzador => 3,
                TipoNavio.Destroyer => 2,
                TipoNavio.Submarino => 1,
                _ => 1
            };
        }
        static string ObterSimboloCelula(TipoCelula celula)
        {
            return celula switch
            {
                TipoCelula.Agua => "~",
                TipoCelula.Erro => "*",
                TipoCelula.Acerto => "X",
                TipoCelula.Submarino => "S",
                TipoCelula.Encouracado => "E",
                TipoCelula.Destroyer => "D",
                TipoCelula.Cruzador => "C",
                TipoCelula.PortaAvioes => "P",
                _ => "?"
            };
        }
        static bool ContemNavio(TipoCelula celula)
        {
            return celula switch
            {
                TipoCelula.Submarino => true,
                TipoCelula.Encouracado => true,
                TipoCelula.Cruzador => true,
                TipoCelula.PortaAvioes => true,
                TipoCelula.Destroyer => true,
                _ => false
            };
        }
        static int CalcularPontosVitoria()
        {
            return 
                quantPortaAvioes * 5 +
                quantEncouracados * 4 +
                quantCruzadores * 3 +
                quantDestroyers * 2 +
                quantSubmarinos * 1 ;
        }
        #endregion
    }
}