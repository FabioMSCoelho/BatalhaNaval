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
        #endregion
        #region Estado do jogo
        static int scorePlayer1 = 0, scorePlayer2 = 0;
        #endregion
        #region Utilidades
        static readonly Random random = new Random();
        #endregion
        #region Configuração
        static int corPlayer1, corPlayer2;
        static string nomePlayer1 = "", nomePlayer2 = "";
        static int quantPortaAvioes = 1;
        static int quantEncouracados = 2;
        static int quantCruzadores = 2;
        static int quantDestroyers = 3;
        static int quantSubmarinos = 3;
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
            int [] coordenadaTiro = new int[2];
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
            IniciaTabuleiro(tabuleiroFrotaP1);
            IniciaTabuleiro(tabuleiroFrotaP2);
            IniciaTabuleiro(tabuleiroRadarP1);
            IniciaTabuleiro(tabuleiroRadarP2);

            while (opcao != 1)
            {
                Console.Clear();
                // MENU
                ImprimirArte(menuArte);
                Space(4);

                Console.WriteLine("[1] Começar novo jogo");
                Console.WriteLine("[2] Continuar");
                Console.WriteLine("[3] Instruções");
                Console.WriteLine("[4] Configurações");
                Console.WriteLine("[5] Sair");
                Space(3);

                //imprime arte do navio
                foreach (string linha in navio)
                {
                    Console.WriteLine(linha);
                }
                Space(2);

                // Lê a opção do usuário
                opcao = LerNumero();
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
                        Space();

                        Console.WriteLine("No início da partida, o campo inimigo estará totalmente oculto.");
                        Console.WriteLine("Você não saberá onde os navios do adversário estão posicionados.");
                        Space();

                        Console.WriteLine("Para atacar, informe a LINHA e a COLUNA do local desejado.");
                        Space();

                        Console.Write("Se você acertar um navio, a posição será marcada com ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(TipoCelula.Acerto);
                        Console.ResetColor();
                        Console.WriteLine(" e você poderá jogar novamente.");
                        Space();

                        Console.Write("Se você errar o ataque, a posição será marcada com ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(TipoCelula.Erro);
                        Console.ResetColor();
                        Console.WriteLine(" e a vez passará para o seu adversário.");
                        Space();

                        Console.WriteLine("O jogo termina quando um dos jogadores destruir todos os navios do oponente.");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Space();
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
                            tabuleiroRadarP1[10, 13] = TipoCelula.Acerto;
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
                            tabuleiroFrotaP1[3, 13] = TipoCelula.Erro;
                            tabuleiroFrotaP1[4, 4] = TipoCelula.Erro;
                            tabuleiroFrotaP1[1, 9] = TipoCelula.Erro;
                            tabuleiroFrotaP1[2, 5] = TipoCelula.Cruzador;
                            tabuleiroFrotaP1[3, 5] = TipoCelula.Cruzador;
                            tabuleiroFrotaP1[11, 8] = TipoCelula.Acerto;
                            tabuleiroFrotaP1[11, 9] = TipoCelula.Acerto;
                            Console.Write("                  Player: Fábio");
                            Console.ResetColor();
                            Console.Write("    <---   jogador da vez");
                            Space(2);
                            Console.ReadKey();
                            Console.WriteLine("Legenda: ");
                            Space();
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
                            Space();
                            Console.WriteLine("logo acima temos uma legenda auto explicativa que lhe informa o que cada simbolo significa");
                            Space(2);
                            Console.ReadKey();
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("                   SUA FROTA");
                            Console.ResetColor();
                            Space();
                            Console.Write("    1  2  3  4  5  6  7  8  9  10 11 12 13");
                            Space();
                            ImprimeTabuleiro(tabuleiroFrotaP1);
                            Space();
                            Console.WriteLine("Este é o campo onde você vizualiza suas embarcações e onde o inimigo lhe atacou");
                            Console.ReadKey();
                            Space(2);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("                   SEU RADAR");
                            Console.ResetColor();
                            Space();
                            
                            ImprimeTabuleiro(tabuleiroRadarP1);
                            Space();
                            Console.WriteLine("Este é o campo onde você Realiza seus ataques");
                            Console.ReadKey();
                            Space();
                            Console.WriteLine("insira a linha em que você quer atacar:");
                            Console.ReadKey();
                            Console.WriteLine("insira a coluna em que você quer atacar:");
                            Console.ReadKey();
                            Space();
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
                            tabuleiroRadarP1[10, 13] = TipoCelula.Agua;
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
                            tabuleiroFrotaP1[3, 13] = TipoCelula.Agua;
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
                        Space(2);
                        Console.Write("Insira a quantidade de submarinos no jogo: ");
                        quantSubmarinos = LerNumero();
                        Console.Write("Insira a quantidade de porta aviões no jogo: ");
                        quantPortaAvioes = LerNumero();
                        Console.Write("Insira a quantidade de cruzadores no jogo: ");
                        quantCruzadores = LerNumero();
                        Console.Write("Insira a quantidade de encouraçados no jogo: ");
                        quantEncouracados = LerNumero();
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
            Space();
            Console.WriteLine("você deseja posicionar sua frota (S/N) ?");
            posicionar = (Console.ReadLine() ?? "").ToUpper();

            if (posicionar == "S")
            {
                Space();
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
                Space();
                Console.WriteLine("Enviando embarcações...");
                Console.ResetColor();
                Thread.Sleep(3000);
                //gera os barcos player 1
                GerarFrota(tabuleiroFrotaP1);
            }

            Console.Clear();
            // configurações iniciais do player 2
            CapturaNomeCor(2);
            Space();
            Console.WriteLine("você deseja posicionar seus navios(S/N) ?");
            posicionar = (Console.ReadLine() ?? "").ToUpper();


            if (posicionar == "S")
            {
                Space();
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
                Space();
                Console.WriteLine("Enviando embarcações...");
                Console.ResetColor();
                Thread.Sleep(3000);                

                // gera os barcos do player 2
                GerarFrota(tabuleiroFrotaP2);
            }

            if (opcao == 1)// opção jogar
            {
                while (scorePlayer1 < CalcPontosVitoria() && scorePlayer2 < CalcPontosVitoria())
                {
                    Console.Clear();

                    //<------------PLAYER1------------->

                    //imprimindo a tela do player
                    ImpressaoTelaJogador(1);

                    // metodo dar tiro (coleta as cordenadas)
                    DarTiro(coordenadaTiro, tabuleiroRadarP1);

                    // metodo que atualiza o tabuleiro após o tiro ser realizando
                    AlteraTabuleiro(tabuleiroFrotaP2, tabuleiroRadarP1, coordenadaTiro);

                    Console.Clear();

                    //<------------PLAYER2------------->

                    //imprime a tela do player 2
                    ImpressaoTelaJogador(2);

                    //metodo dar tiro (coleta as cordenadas)
                    DarTiro(coordenadaTiro, tabuleiroRadarP2);

                    //metodo que atualiza o a tabuleiro após o tiro ser realizando
                    AlteraTabuleiro(tabuleiroFrotaP1, tabuleiroRadarP2, coordenadaTiro);

                }
            }
        }
        #endregion

    //metodos
        #region Tabuleiro
        //gera os tabuleiros com "agua" em todas as posições
        static void IniciaTabuleiro(TipoCelula[,] tabuleiro)
        {
            for (int l = 0; l < tabuleiro.GetLength(0); l++)
            {
                for (int c = 0; c < tabuleiro.GetLength(1); c++)
                {
                    tabuleiro[l, c] = TipoCelula.Agua;
                }
            }
        }
        //verifica se o tiro acertou e altera os icones nos tabuleiros
        static void AlteraTabuleiro(TipoCelula[,] playerAtacado, TipoCelula[,] playerAtacante, int[] coordenadaTiro)
        {
            TipoCelula alvo = playerAtacado[coordenadaTiro[0], coordenadaTiro[1]];

            // ACERTO
            if (alvo == TipoCelula.Submarino || alvo == TipoCelula.Encouracado || alvo == TipoCelula.Cruzador || alvo == TipoCelula.PortaAvioes)
            {
                playerAtacante[coordenadaTiro[0], coordenadaTiro[1]] = TipoCelula.Acerto;
                playerAtacado[coordenadaTiro[0], coordenadaTiro[1]] = TipoCelula.Acerto;

                Console.Clear();

                if (playerAtacado == tabuleiroFrotaP2){
                    ImpressaoTelaJogador(1);
                    scorePlayer1++;
                }
                else if (playerAtacado == tabuleiroFrotaP1){
                    ImpressaoTelaJogador(2);
                    scorePlayer2++;
                }
                
                Console.ForegroundColor = ConsoleColor.Green;
                Space();
                Console.WriteLine("\n💥 Você acertou um navio! Jogue novamente.");
                Console.ResetColor();

                Thread.Sleep(1500);

                DarTiro(coordenadaTiro, playerAtacante);
                AlteraTabuleiro(playerAtacado, playerAtacante, coordenadaTiro);
            }

            // ERRO
            else if (alvo == TipoCelula.Agua)
            {
                playerAtacante[coordenadaTiro[0], coordenadaTiro[1]] = TipoCelula.Erro;
                playerAtacado[coordenadaTiro[0], coordenadaTiro[1]] = TipoCelula.Erro;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Space();
                Console.WriteLine("\n💧 Você errou o disparo! Vez do adversário.");
                Console.ResetColor();

                Thread.Sleep(2000);
            }
        }
        #endregion
        #region Navios
        static void GerarFrota(TipoCelula[,] tabuleiro)
        {
            GerarBarco(quantPortaAvioes, tabuleiro, TipoNavio.PortaAvioes);
            GerarBarco(quantEncouracados, tabuleiro, TipoNavio.Encouracado);
            GerarBarco(quantCruzadores, tabuleiro, TipoNavio.Cruzador);
            GerarBarco(quantCruzadores, tabuleiro, TipoNavio.Destroyer);
            GerarBarco(quantSubmarinos, tabuleiro, TipoNavio.Submarino);
        }
        static void GerarBarco(int quantidade, TipoCelula[,] tabuleiro, TipoNavio tipo)
        {
            TipoCelula letra = LetraNavio(tipo);
            int tamanho = TamanhoNavio(tipo);

            for (int i = 0; i < quantidade; i++)
            {
                bool colocado = false;

                while (!colocado)
                {
                    int linha = random.Next(0, TAM_TABULEIRO);
                    int coluna = random.Next(0, TAM_TABULEIRO);
                    bool horizontal = random.Next(2) == 0;

                    colocado = PosicionaBarco(tabuleiro, linha, coluna, tamanho, letra, horizontal);
                }
            }
        }
        static bool PosicionaBarco(TipoCelula[,] tabuleiro, int linha, int coluna, int tamanho, TipoCelula barco, bool horizontal)
        {
            int tamanhoTabuleiro = tabuleiro.GetLength(0);

            // Verifica se o barco cabe no tabuleiro
            if (horizontal)
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
                int l = horizontal ? linha : linha + i;
                int c = horizontal ? coluna + i : coluna;

                if (tabuleiro[l, c] != TipoCelula.Agua)
                    return false;
            }

            // Posiciona o barco
            for (int i = 0; i < tamanho; i++)
            {
                int l = horizontal ? linha : linha + i;
                int c = horizontal ? coluna + i : coluna;

                tabuleiro[l, c] = barco;
            }

            return true;
        }
        static void PosicionarFrotaManual(TipoCelula[,] tabuleiro)
        {
            PosicionaBarcoManual(quantPortaAvioes, TipoNavio.PortaAvioes, tabuleiro);
            PosicionaBarcoManual(quantEncouracados, TipoNavio.Encouracado, tabuleiro);
            PosicionaBarcoManual(quantCruzadores, TipoNavio.Cruzador, tabuleiro);
            PosicionaBarcoManual(quantDestroyers, TipoNavio.Destroyer, tabuleiro);
            PosicionaBarcoManual(quantSubmarinos, TipoNavio.Submarino, tabuleiro);
        }
        static void PosicionaBarcoManual(int quantidade, TipoNavio tipo, TipoCelula[,] tabuleiro)
        {
            TipoCelula letra = LetraNavio(tipo);
            int tamanho = TamanhoNavio(tipo);
            string nome = tipo.ToString();

            for (int i = 0; i < quantidade; i++)
            {
                bool colocado = false;

                while (!colocado)
                {
                    Console.Clear();
                    InsereCabecalho("frota");
                    ImprimeTabuleiro(tabuleiro);

                    Console.WriteLine($"Posicionando {nome} (tamanho {tamanho})");

                    Console.Write("Linha inicial: ");
                    int linha = LerNumero() - 1;

                    Console.Write("Coluna inicial: ");
                    int coluna = LerNumero() - 1;

                    bool horizontal = true;

                    if (tamanho > 1)
                    {
                        Console.Write("Horizontal? (s/n): ");
                        horizontal = (Console.ReadLine()?.Trim().ToLower() == "s");
                    }

                    colocado = PosicionaBarco(tabuleiro, linha, coluna, tamanho, letra, horizontal);

                    if (!colocado)
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
        static void DarTiro(int[] coordenadaTiro, TipoCelula[,] tabuleiro)
        {
            Space(2);
            Console.Write("insira a linha em que você quer atacar: ");
            coordenadaTiro[0] = LerNumero() - 1;
            Console.Write("insira a coluna em que você quer atacar: ");
            coordenadaTiro[1] = LerNumero() - 1;

            ValidarLimiteTabuleiro(coordenadaTiro, tabuleiro);
            ValidarRepeticaoJogada(coordenadaTiro, tabuleiro);
        }
        //tratamento de erros (explosão da matriz)
        static void ValidarLimiteTabuleiro(int[] coordenadaTiro, TipoCelula[,] player)
        {
            while (coordenadaTiro[0] < 0 || coordenadaTiro[0] >= TAM_TABULEIRO || coordenadaTiro[1] < 0 || coordenadaTiro[1] >= TAM_TABULEIRO)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("A posição esta fora do radar, escolha outra");
                Console.ResetColor();
                DarTiro(coordenadaTiro, player);
            }
        }
        //tratamento de erros (repetição de jogada)
        static void ValidarRepeticaoJogada(int[] coordenadaTiro, TipoCelula[,] player)
        {
            while (player[coordenadaTiro[0], coordenadaTiro[1]] != TipoCelula.Agua)
            {
                Console.WriteLine("você já disparou neste quadrante, escolha outro");
                DarTiro(coordenadaTiro, player);
            }
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
                Space();
            }
        }
        static void CapturaNomeCor(int num)
        {
            Console.WriteLine("player " + num);
            Space();

            Console.Write("Insira o seu nome: ");
            string nomeDigitado = Console.ReadLine() ?? "";

            Space();
            Console.WriteLine("Escolha uma cor: ");
            Space();

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

            Space();

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

            Space();

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

            Space();

            int corEscolhida; 
            corEscolhida = LerNumero();
            
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
        static void ImpressaoTelaJogador(int playerdavez)
        {
            if (playerdavez == 1)
            {
                PlayerLegenda(1);
                InsereCabecalho("frota");
                ImprimeTabuleiro(tabuleiroFrotaP1);
                InsereCabecalho("radar");
                ImprimeTabuleiro(tabuleiroRadarP1);
                Space();
            }
            else if (playerdavez == 2)
            {
                PlayerLegenda(2);
                InsereCabecalho("frota");
                ImprimeTabuleiro(tabuleiroFrotaP2);
                InsereCabecalho("radar");
                ImprimeTabuleiro(tabuleiroRadarP2);
                Space();
            }
        }
        //exibe nome do player da vez e a legenda dos tabuleiros
        static void PlayerLegenda(int numeroPlayer)
        {
            Console.Write("                  Player: ");
            if (numeroPlayer == 1)
            {
                Console.ForegroundColor = SelecaoCorPlayer(corPlayer1);  
                Console.Write(nomePlayer1);
            }
            else if (numeroPlayer == 2)
            {
                Console.ForegroundColor = SelecaoCorPlayer(corPlayer2);                
                Console.Write(nomePlayer2);
            }
            Console.ResetColor();
            Space(2);
            Console.WriteLine("Legenda: ");
            Space();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(CelulaSimbolo(TipoCelula.Agua));
            Console.ResetColor();
            Console.WriteLine(" agua");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(CelulaSimbolo(TipoCelula.Erro));
            Console.ResetColor();
            Console.WriteLine(" errou");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(CelulaSimbolo(TipoCelula.Acerto));
            Console.ResetColor();
            Console.WriteLine(" acertou");
            Space(2);
        }
        //cabeçalho dos tabuleiros
        static void InsereCabecalho(string frotaOuRadar)
        {
            if (frotaOuRadar == "frota")
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("                   SUA FROTA");
            }
            else if (frotaOuRadar == "radar")
            {
                Space(2);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("                   SEU RADAR");
            }
            Console.ResetColor();
            Space();
            Console.Write("    1  2  3  4  5  6  7  8  9  10 11 12 13");
            Space();

        }
        static void ImprimeTabuleiro(TipoCelula[,] player)
        {
            for (int l = 0; l < player.GetLength(0); l++)
            {
                Console.Write((l + 1).ToString("00"));
                for (int c = 0; c < player.GetLength(1); c++)
                {
                    if (player[l, c] == TipoCelula.Agua)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    else if (player[l, c] == TipoCelula.Acerto)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (player[l, c] == TipoCelula.Erro)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    Console.Write("  " + CelulaSimbolo(player[l, c]));
                    Console.ResetColor();
                }
                Space();
            }
        }
        #endregion
        #region Auxiliares
        //valida e captura o numero digitado
        static int LerNumero()
        {
            int numero;

            while (!int.TryParse(Console.ReadLine(), out numero))
            {
                Console.WriteLine("Digite um número válido:");
            }

            return numero;
        }
        //insere os espaços na interface 
        static void Space() 
        {
            Console.WriteLine();
        }
        static void Space(int linhas) 
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
        static TipoCelula LetraNavio(TipoNavio tipo)
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
        static int TamanhoNavio(TipoNavio tipo)
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
        static string CelulaSimbolo(TipoCelula celula)
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
        static int CalcPontosVitoria()
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