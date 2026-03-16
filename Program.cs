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
        static Dictionary<int, int> placar = new Dictionary<int, int>()
        {
            {1, 0},
            {2, 0}
        };
        #region Estado do jogo
        
        #endregion
        #region Utilidades
        static readonly Random random = new Random();
        #endregion
        #region Configuração
        static int corPlayer1, corPlayer2;
        static string nomePlayer1 = "", nomePlayer2 = "";
        static int quantPortaAvioes = 1; //1
        static int quantEncouracados = 2; //2
        static int quantCruzadores = 2; //2
        static int quantDestroyers = 3; //3
        static int quantSubmarinos = 1; //3
        #endregion
        #region Artes
        static readonly string[] navio = new string[]
            {
                "                                       |__",
                "                                       |\\/",
                "                                       ---",
                "                                       / | [",
                "                                !      | |||",
                "                              _/|     _/|-++'",
                "                          +  +--|    |--|--|_ |-",
                "                       { /|__|  |/\\__|  |--- |||__/",
                "                      +---------------___[}-_===_.'____                 /\\",
                "                  ____`-' ||___-{]_| _[}-  |     |_[___\\==--            \\/   _",
                "    __..._____--==/___]_|__|_____________________________[___\\==--____,------' .7",
                "   |                                                                     BB-61/",
                "    \\_________________________________________________________________________|",
                "   ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
            };
        static readonly LinhaColorida[] menuArte = new LinhaColorida[]
            {
                new LinhaColorida("      __________         __    __  .__         ", ConsoleColor.Cyan),
                new LinhaColorida("  _________.__    .__        ", ConsoleColor.Red),
                new LinhaColorida("      \\______   \\_____ _/  |__/  |_|  |   ____ ", ConsoleColor.Cyan),
                new LinhaColorida(" /   _____/|  |__ |__|_____  ", ConsoleColor.Red),
                new LinhaColorida("       |    |  _/\\__  \\\\   __\\   __\\  | _/ __ \\", ConsoleColor.Cyan),
                new LinhaColorida(" \\_____  \\ |  |  \\|  \\  __ \\ ", ConsoleColor.Red),
                new LinhaColorida("       |    |   \\ / __ \\|  |  |  | |  |_\\  ___/", ConsoleColor.Cyan),
                new LinhaColorida(" /        \\|   Y  \\  |  |_| >", ConsoleColor.Red),
                new LinhaColorida("       |______  /(____  /__|  |__| |____/\\___ \\", ConsoleColor.Cyan),
                new LinhaColorida("/_______  /|___|  /__|   __/", ConsoleColor.Red),
                new LinhaColorida("              \\/      \\/                     \\/", ConsoleColor.Cyan),
                new LinhaColorida("        \\/      \\/   |__|   ", ConsoleColor.Red)
            };
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
            while (true)
            {
                switch (ExibirMenu())
                {
                    case 1:
                        Novojogo();
                        break;
                    case 2:
                        ExibirInstrucoes();
                        break;
                    case 3:
                        ConfigurarQuantidadeNavios();
                        break;
                    case 4:
                        SairDoJogo();
                    break;
                    default:
                    Console.WriteLine("Opção inválida");
                    break;
                }
            }      
        }
        #endregion

        //metodos
        #region fluxo do jogo
        static void Novojogo()
        {
            placar[1] = 0;
            placar[2] = 0;
            Coordenada posicaoTiro = new Coordenada();
            bool jogoAtivo = true;

            InicializarTabuleiro(tabuleiroFrotaP1);
            InicializarTabuleiro(tabuleiroFrotaP2);
            InicializarTabuleiro(tabuleiroRadarP1);
            InicializarTabuleiro(tabuleiroRadarP2);
            ConfigurarJogador(1);
            ConfigurarJogador(2);

            while (jogoAtivo)
            {
                // turno do player 1
                RealizarTurno(1, tabuleiroRadarP1, tabuleiroFrotaP2, posicaoTiro);

                if (VerificarVitoria())
                {
                    jogoAtivo = false;
                    break;
                }

                // turno do player 2
                RealizarTurno(2, tabuleiroRadarP2, tabuleiroFrotaP1, posicaoTiro);

                if (VerificarVitoria())
                {
                    jogoAtivo = false;
                }
            }

            ExibirVitoria();
        }
        static void SairDoJogo()
        {
            Console.WriteLine("Saindo do jogo...");
            Thread.Sleep(1000); // Pequena pausa para o usuário ver a mensagem
            Environment.Exit(0); // encerra o programa
        }
        static bool VerificarVitoria()
        {
            return placar[1] >= CalcularPontosVitoria() ||
                placar[2] >= CalcularPontosVitoria();
        }
        static void ExibirVitoria()
        {
            Console.Clear();

            string vencedor =
                placar[1] >= CalcularPontosVitoria()
                ? nomePlayer1
                : nomePlayer2;

            EscreverColorido("🏆 VITÓRIA!", ConsoleColor.Green);
            EscreverColorido($"{vencedor} destruiu toda a frota inimiga!", ConsoleColor.Green);
            
            Console.ReadKey();
        }
        #endregion
        #region Configuraçao do jogo
        static void ConfigurarJogador(int jogador)
        {
            string posicionar = "N";
            TipoCelula[,] tabuleiro = jogador == 1 ? tabuleiroFrotaP1:tabuleiroFrotaP2;

            CapturaNomeCor(jogador);

            PularLinha();

            Console.WriteLine("você deseja posicionar sua frota (S/N) ?");
            posicionar = (Console.ReadLine() ?? "").ToUpper();

            if (posicionar == "S")
            {
                PularLinha();
                EscreverColorido("Pressione qualquer tecla para iniciar o posicionamento", ConsoleColor.DarkGray);
                Console.ReadKey();
                Console.Clear();
                PosicionarFrotaManual(tabuleiro);
                Thread.Sleep(800);
                EscreverColorido("Agora é a vez do segundo player", SelecaoCorPlayer(corPlayer2));
                
                Thread.Sleep(2000);
            }            

            else if (posicionar == "N")
            {
                PularLinha();
                EscreverColorido("Enviando embarcações...", ConsoleColor.DarkGray);

                Thread.Sleep(3000);
                //gera os barcos player 1
                GerarFrota(tabuleiro);
            }

            Console.Clear();
        }

        static void ConfigurarQuantidadeNavios()
        {
            EscreverColorido("              configurações", ConsoleColor.DarkRed);

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
            EscreverColorido("Retornando ao menu...", ConsoleColor.DarkGray);

            Thread.Sleep(2000);
            Console.Clear();
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
            EscreverColorido(nomeDigitado, ConsoleColor.Red, false);

            Console.Write("             4 - ");
            EscreverColorido(nomeDigitado, ConsoleColor.Cyan, false);

            Console.Write("             7 - ");
            EscreverColorido(nomeDigitado, ConsoleColor.Blue);

            PularLinha();

            Console.Write("2 - ");
            EscreverColorido(nomeDigitado, ConsoleColor.Yellow, false);

            Console.Write("             5 - ");
            EscreverColorido(nomeDigitado, ConsoleColor.DarkGreen, false);

            Console.Write("             8 - ");
            EscreverColorido(nomeDigitado, ConsoleColor.DarkGray);

            PularLinha();

            Console.Write("3 - ");
            EscreverColorido(nomeDigitado, ConsoleColor.Green, false);

            Console.Write("             6 - ");
            EscreverColorido(nomeDigitado, ConsoleColor.Gray, false);
            
            Console.Write("             9 - ");
            EscreverColorido(nomeDigitado, ConsoleColor.DarkMagenta);
            
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
        #endregion
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
        static bool ProcessarTiro(TipoCelula[,] tabuleiroDefensor, TipoCelula[,] tabuleiroRadar, Coordenada posicaoTiro)
        {
            TipoCelula alvo = tabuleiroDefensor[posicaoTiro.Linha, posicaoTiro.Coluna];

            // ACERTO
            if (ContemNavio(alvo))
            {   
                tabuleiroDefensor[posicaoTiro.Linha, posicaoTiro.Coluna] = TipoCelula.Acerto;
                tabuleiroRadar[posicaoTiro.Linha, posicaoTiro.Coluna] = TipoCelula.Acerto;
                return true;
            }

            // ERRO
            else if (alvo == TipoCelula.Agua)
            {
                tabuleiroDefensor[posicaoTiro.Linha, posicaoTiro.Coluna] = TipoCelula.Erro;
                tabuleiroRadar[posicaoTiro.Linha, posicaoTiro.Coluna] = TipoCelula.Erro;                
            }
             return false;
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
                        EscreverColorido("Não foi possível posicionar o barco.", ConsoleColor.Red);
                        Console.ReadLine();
                    }
                }
            }
        }
        
        #endregion
        #region Mecânicas do jogo
        static void RealizarTurno(int jogadorAtual, TipoCelula[,] tabuleiroRadar, TipoCelula[,] tabuleiroDefensor, Coordenada posicaoTiro)
        {
            bool acertou;
            do
            {
                ExibirTelaJogador(jogadorAtual);
                CapturarCoordenadaTiro(ref posicaoTiro, tabuleiroRadar);
                
                acertou = ProcessarTiro(tabuleiroDefensor, tabuleiroRadar, posicaoTiro);
                
                Console.Clear();
                ExibirTelaJogador(jogadorAtual);

                if (VerificarVitoria())
                {
                    EscreverColorido("💥 ACERTOU O ÚLTIMO NAVIO!", ConsoleColor.Green);
                    Thread.Sleep(2000);
                    break;
                }
                
                PularLinha();
                if (acertou)
                {
                    placar[jogadorAtual]++;
                    EscreverColorido("💥 Você acertou um navio! Jogue novamente.", ConsoleColor.Green);
                    Thread.Sleep(2000);
                }
                else
                {
                    EscreverColorido("💧 Você errou o disparo! Vez do adversário.", ConsoleColor.Yellow);
                    Thread.Sleep(2000);
                }
                        
                Console.Clear();
            }
            while(acertou);
        }
        //coleta as coordenadas do ataque, linha e coluna
        static void CapturarCoordenadaTiro(ref Coordenada posicaoTiro, TipoCelula[,] tabuleiro)
        {
            bool tiroValido = false;
            while (!tiroValido)
            {
                PularLinha(2);
                Console.Write("insira a linha em que você quer atacar: ");
                posicaoTiro.Linha = LerInteiro() - 1;
                Console.Write("insira a coluna em que você quer atacar: ");
                posicaoTiro.Coluna = LerInteiro() - 1;

                    //tratamento de erros (explosão da matriz)
                    if(posicaoTiro.Linha < 0 || posicaoTiro.Linha >= TAM_TABULEIRO ||
                        posicaoTiro.Coluna < 0 || posicaoTiro.Coluna >= TAM_TABULEIRO)
                        {
                            EscreverColorido("A posição está fora do radar, escolha outra.", ConsoleColor.Red);

                            continue;
                        }
                    //tratamento de erros (repetição de jogada)
                    if(tabuleiro[posicaoTiro.Linha, posicaoTiro.Coluna] != TipoCelula.Agua)
                        {
                            EscreverColorido("Você já disparou neste quadrante, escolha outro.", ConsoleColor.DarkRed);
                            continue;
                        }

                tiroValido = true;
            }
        
        }
        #endregion
        #region Interface
        static int ExibirMenu()
        {
            Console.Clear();
            PularLinha();
            ImprimirArte(menuArte);
            PularLinha(5);

            Console.WriteLine("     [1] Começar novo jogo");
            Console.WriteLine("     [2] Instruções");
            Console.WriteLine("     [3] Configurações");
            Console.WriteLine("     [4] Sair");
            PularLinha(4);

            //imprime arte do navio
            foreach (string linha in navio)
            {
                Console.WriteLine(linha);
            }
            PularLinha(2);

            // Lê a opção do usuário
            int opcao = LerInteiro();
            Console.Clear();
            return opcao;
        }
        static void ExibirInstrucoes()
        {
            string desejaConhecer;
            EscreverColorido("                        Instruções", ConsoleColor.DarkCyan);

            Console.WriteLine("O objetivo do jogo é destruir todos os navios do seu oponente.");
            PularLinha();

            Console.WriteLine("No início da partida, o campo inimigo estará totalmente oculto.");
            Console.WriteLine("Você não saberá onde os navios do adversário estão posicionados.");
            PularLinha();

            Console.WriteLine("Para atacar, informe a LINHA e a COLUNA do local desejado.");
            PularLinha();

            Console.Write("Se você acertar um navio, a posição será marcada com ");
            EscreverColorido(ObterSimboloCelula(TipoCelula.Acerto), ConsoleColor.Red, false);

            Console.WriteLine(" e você poderá jogar novamente.");
            PularLinha();

            Console.Write("Se você errar o ataque, a posição será marcada com ");
            EscreverColorido(ObterSimboloCelula(TipoCelula.Erro), ConsoleColor.Yellow, false);
            
            Console.WriteLine(" e a vez passará para o seu adversário.");
            PularLinha();
            EscreverColorido("O jogo termina quando um dos jogadores destruir todos os navios do oponente.", ConsoleColor.DarkGray);
            PularLinha();
            Console.WriteLine("você deseja Conhecer a interface do jogo (S/N) ?");
            Console.ResetColor();
            desejaConhecer = (Console.ReadLine() ?? "").ToUpper();
            Console.Clear();
            if (desejaConhecer == "S")
            {
                DemonstrarInterface();
            }
        }
        static void DemonstrarInterface()
        {
            TipoCelula[,] tabuleiroRadarDemo = new TipoCelula[TAM_TABULEIRO, TAM_TABULEIRO];
            TipoCelula[,] tabuleiroFrotaDemo = new TipoCelula[TAM_TABULEIRO, TAM_TABULEIRO];
            InicializarTabuleiro(tabuleiroRadarDemo);
            InicializarTabuleiro(tabuleiroFrotaDemo);

            tabuleiroRadarDemo[0, 0] = TipoCelula.Erro;
            tabuleiroRadarDemo[7, 8] = TipoCelula.Erro;
            tabuleiroRadarDemo[3, 5] = TipoCelula.Erro;
            tabuleiroRadarDemo[9, 3] = TipoCelula.Erro;
            tabuleiroRadarDemo[10, 10] = TipoCelula.Acerto;
            tabuleiroRadarDemo[10, 11] = TipoCelula.Acerto;
            tabuleiroRadarDemo[10, 12] = TipoCelula.Acerto;
            tabuleiroRadarDemo[8, 7] = TipoCelula.Acerto;
            tabuleiroFrotaDemo[1, 1] = TipoCelula.Submarino;
            tabuleiroFrotaDemo[3, 9] = TipoCelula.Submarino;
            tabuleiroFrotaDemo[7, 8] = TipoCelula.PortaAvioes;
            tabuleiroFrotaDemo[7, 9] = TipoCelula.PortaAvioes;
            tabuleiroFrotaDemo[7, 10] = TipoCelula.Acerto;
            tabuleiroFrotaDemo[7, 11] = TipoCelula.PortaAvioes;
            tabuleiroFrotaDemo[7, 12] = TipoCelula.PortaAvioes;
            tabuleiroFrotaDemo[10, 4] = TipoCelula.Encouracado;
            tabuleiroFrotaDemo[11, 4] = TipoCelula.Encouracado;
            tabuleiroFrotaDemo[0, 5] = TipoCelula.Cruzador;
            tabuleiroFrotaDemo[1, 5] = TipoCelula.Cruzador;
            tabuleiroFrotaDemo[3, 12] = TipoCelula.Erro;
            tabuleiroFrotaDemo[4, 4] = TipoCelula.Erro;
            tabuleiroFrotaDemo[1, 9] = TipoCelula.Erro;
            tabuleiroFrotaDemo[2, 5] = TipoCelula.Cruzador;
            tabuleiroFrotaDemo[3, 5] = TipoCelula.Cruzador;
            tabuleiroFrotaDemo[11, 8] = TipoCelula.Acerto;
            tabuleiroFrotaDemo[11, 9] = TipoCelula.Acerto;
            Console.Write("                  Player: Fábio");
            Console.ResetColor();
            Console.Write("    <---   jogador da vez");
            PularLinha(2);
            Console.ReadKey();
            Console.WriteLine("Legenda: ");
            PularLinha();
            EscreverColorido(ObterSimboloCelula(TipoCelula.Agua), ConsoleColor.Cyan, false);
            Console.WriteLine(" agua");
            EscreverColorido(ObterSimboloCelula(TipoCelula.Erro), ConsoleColor.Yellow, false);
            Console.WriteLine(" errou");
            EscreverColorido(ObterSimboloCelula(TipoCelula.Acerto), ConsoleColor.Red, false);
            Console.WriteLine(" acertou");
            PularLinha();
            Console.WriteLine("logo acima temos uma legenda auto explicativa que lhe informa o que cada simbolo significa");
            PularLinha(2);
            Console.ReadKey();
            EscreverColorido("                   SUA FROTA", ConsoleColor.DarkGreen);
            PularLinha();
            Console.Write("    1  2  3  4  5  6  7  8  9  10 11 12 13");
            PularLinha();
            ImprimeTabuleiro(tabuleiroFrotaDemo);
            PularLinha();
            Console.WriteLine("Este é o campo onde você vizualiza suas embarcações e onde o inimigo lhe atacou");
            Console.ReadKey();
            PularLinha(2);
            EscreverColorido("                   SEU RADAR", ConsoleColor.Red);
            PularLinha();
            
            ImprimeTabuleiro(tabuleiroRadarDemo);
            PularLinha();
            Console.WriteLine("Este é o campo onde você Realiza seus ataques");
            Console.ReadKey();
            PularLinha();
            Console.WriteLine("insira a linha em que você quer atacar:");
            Console.ReadKey();
            Console.WriteLine("insira a coluna em que você quer atacar:");
            Console.ReadKey();
            PularLinha();
            EscreverColorido("Presione qualquer tecla para voltar ao menu", ConsoleColor.DarkGray);
            Console.ReadKey();
            Console.Clear();
        }
        static void ImprimirArte(LinhaColorida[] arte)
        {
            for (int i = 0; i < arte.Length; i += 2)
            {
                // imprime a primeira linha do par
                EscreverColorido(arte[i].Texto, arte[i].Cor, false);
                
                // imprime a segunda linha do par, se existir
                if (i + 1 < arte.Length)
                {
                    EscreverColorido(arte[i + 1].Texto, arte[i + 1].Cor, false);
                }

                // quebra de linha após o par
                PularLinha();
            }
        }
    
        //metodo que reune a chamada de um grupo de metodos que constituem a tela do player 
        static void ExibirTelaJogador(int jogadorAtual)
        {
            TipoCelula[,] frota = jogadorAtual == 1 ? tabuleiroFrotaP1 : tabuleiroFrotaP2;
            TipoCelula[,] radar = jogadorAtual == 1 ? tabuleiroRadarP1 : tabuleiroRadarP2;

            ExibirLegendaJogador(jogadorAtual);
            ExibirCabecalhoTabuleiro("frota");
            ImprimeTabuleiro(frota);
            ExibirCabecalhoTabuleiro("radar");
            ImprimeTabuleiro(radar);
            PularLinha();
        }
        //exibe nome do player da vez e a legenda dos tabuleiros
        static void ExibirLegendaJogador(int idJogador)
        {   
            Console.Write("                  Player: ");
            if (idJogador == 1)
            {
                EscreverColorido(nomePlayer1, SelecaoCorPlayer(corPlayer1));
            }
            else if (idJogador == 2)
            {
                EscreverColorido(nomePlayer2, SelecaoCorPlayer(corPlayer2));              
            }
            PularLinha(2);
            Console.WriteLine("Legenda: ");
            PularLinha();
            EscreverColorido(ObterSimboloCelula(TipoCelula.Agua), ConsoleColor.Cyan, false);
            Console.WriteLine(" agua");
            EscreverColorido(ObterSimboloCelula(TipoCelula.Erro), ConsoleColor.Yellow,false);
            Console.WriteLine(" errou");
            EscreverColorido(ObterSimboloCelula(TipoCelula.Acerto), ConsoleColor.Red, false);
            Console.WriteLine(" acertou");
            PularLinha(2);
        }
        //cabeçalho dos tabuleiros
        static void ExibirCabecalhoTabuleiro(string frotaOuRadar)
        {
            if (frotaOuRadar == "frota")
            {
                EscreverColorido("                   SUA FROTA", ConsoleColor.DarkGreen);
            }
            else if (frotaOuRadar == "radar")
            {
                PularLinha(2);
                EscreverColorido("                   SEU RADAR", ConsoleColor.DarkRed);
            }
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
                    ConsoleColor cor = ConsoleColor.Gray;

                    if (tabuleiro[linha, coluna] == TipoCelula.Agua)
                    {
                        cor = ConsoleColor.Cyan;
                    }
                    else if (tabuleiro[linha, coluna] == TipoCelula.Acerto)
                    {
                        cor = ConsoleColor.Red;
                    }
                    else if (tabuleiro[linha, coluna] == TipoCelula.Erro)
                    {
                        cor = ConsoleColor.Yellow;
                    }
                    EscreverColorido("  " + ObterSimboloCelula(tabuleiro[linha, coluna]), cor, false);
                }
                PularLinha();
            }
        }
        #endregion
        #region Auxiliares
        static void EscreverColorido(string mensagem, ConsoleColor cor, bool quebrarLinha = true)
        {
            Console.ForegroundColor = cor;

            if (quebrarLinha)
                Console.WriteLine(mensagem);
            else
                Console.Write(mensagem);

            Console.ResetColor();
        }
        //captura e valida o numero digitado
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