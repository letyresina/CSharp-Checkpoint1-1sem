using Figgle;
using System;
using System.Collections.Generic;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.ForegroundColor = ConsoleColor.White; //Cor do Texto
Console.BackgroundColor = ConsoleColor.DarkMagenta; // Cor de fundo do texto
Console.Clear();//Faz do fundo do texto o terminal da mesma cor


//Dicionário que armazena as estatísticas de vitórias, empates e derrotas de cada jogador.
Dictionary<string, (int vitórias, int empates, int derrotas)> jogadores = new Dictionary<string, (int, int, int)>();
//Dicionário que mapeia os números das jogadas para suas representações em texto (Pedra, Papel, Tesoura).
Dictionary<int, string> jogadas = new Dictionary<int, string>
{
    { 0, "Pedra" },
    { 1, "Papel" },
    { 2, "Tesoura" }
};
int vitoriasComputador = 0, derrotasComputador = 0, empatesComputador = 0;

Console.WriteLine(FiggleFonts.Standard.Render("Jokempo Menos Um!"));
Console.WriteLine("1 - Sim ou 0 - Não");
int continuar = ValidaEscolha(0, 1);
List<int> armaCarregada = RecarregaArma();

#region Fluxo Principal
//Loop principal que mantém o jogo em execução até que o usuário escolha sair.
while (continuar == 1)
{
    string nomeJogador = ObterNomeJogador();

    // Se o jogador não existe no dicionário, cria um novo registro para ele.
    if (!jogadores.ContainsKey(nomeJogador))
    {
        jogadores[nomeJogador] = (0, 0, 0);
        //Recarrega a arma se iniciar com um novo jogador
        armaCarregada = RecarregaArma();
    }
    Console.Clear();
    continuar = 3;

    while (continuar == 3)
    {
        Console.WriteLine(FiggleFonts.Slant.Render($"{nomeJogador} VS Computador"));
        Console.WriteLine("O computador já fez suas escolhas.");
        Console.WriteLine("Opções: 0 - Pedra, 1 - Papel, 2 - Tesoura");

        int escolhaComputador1 = new Random().Next(0, 3);
        int escolhaComputador2 = new Random().Next(0, 3);

        //Obtêm as escolhas do jogador
        Console.Write("\nEscolha sua primeira opção: ");
        int escolhaJogador1 = ValidaEscolha(0, 1, 2);
        Console.Write("\nEscolha sua segunda opção: ");
        int escolhaJogador2 = ValidaEscolha(0, 1, 2);

        Console.Clear();
        Console.WriteLine($"O computador escolheu: {jogadas[escolhaComputador1]} e {jogadas[escolhaComputador2]}");
        Console.Write($"\n{nomeJogador} escolha qual jogada deseja usar (1 - {jogadas[escolhaJogador1]} ou 2 - {jogadas[escolhaJogador2]}): ");
        int opcaoJogador = ValidaEscolha(1, 2);

        int jogadaFinalJogador = (opcaoJogador == 1) ? escolhaJogador1 : escolhaJogador2;
        int jogadaFinalComputador = EscolherMelhorJogadaComputador(escolhaComputador1, escolhaComputador2, escolhaJogador1, escolhaJogador2);

        Console.WriteLine($"\nVocê escolheu: {jogadas[jogadaFinalJogador]}");
        Console.WriteLine($"O computador escolheu: {jogadas[jogadaFinalComputador]}");

        /**
         * Verifica o resultado da partida (empate, vitória ou derrota) e atualiza as estatísticas.
         * Caso ocorra uma vitória jogamos a roleta russa
         */
        if (jogadaFinalJogador == jogadaFinalComputador)
        {
            Console.WriteLine("\nEmpate!");
            jogadores[nomeJogador] = (jogadores[nomeJogador].vitórias, jogadores[nomeJogador].empates + 1, jogadores[nomeJogador].derrotas);
            empatesComputador++;
        }
        //Caso o pc perca, ele joga a roleta russa
        else if (Vence(jogadaFinalJogador, jogadaFinalComputador))
        {
            Console.WriteLine(FiggleFonts.Small.Render("Você venceu!"));
            jogadores[nomeJogador] = (jogadores[nomeJogador].vitórias + 1, jogadores[nomeJogador].empates, jogadores[nomeJogador].derrotas);
            derrotasComputador++;
            if (SimularRoletaRussa("Computador", armaCarregada))
            {
                armaCarregada = RecarregaArma();
                Console.WriteLine("\nJogar de novo? 1 - Sim  0 - Não");
                continuar = ValidaEscolha(0, 1);
                Console.Clear();
                break;
            }
        }
        // Caso o jogador perca, ele joga a roleta russa
        else
        {
            Console.WriteLine(FiggleFonts.Small.Render("Computador venceu!"));
            jogadores[nomeJogador] = (jogadores[nomeJogador].vitórias, jogadores[nomeJogador].empates, jogadores[nomeJogador].derrotas + 1);
            vitoriasComputador++;
            if (SimularRoletaRussa(nomeJogador, armaCarregada))
            {
                jogadores.Remove(nomeJogador);
                armaCarregada = RecarregaArma();
                Console.WriteLine("\nJogar de novo? 1 - Sim  0 - Não");
                continuar = ValidaEscolha(0, 1);
                Console.Clear();
                break;
            }
        }

        //Exibição do menu após fim da partida
        Console.WriteLine("\n2 - Estatísticas  3 - Continuar");
        continuar = ValidaEscolha(2, 3);
        Console.Clear();
        if (continuar == 2)
        {
            ListarEstatisticasJogadores();
            Console.WriteLine("\n3 - Continuar");
            continuar = ValidaEscolha(3);
            Console.Clear();
        }
    }
}
Console.WriteLine(FiggleFonts.Standard.Render("Tchau!"));
#endregion

#region Metodos

/// <summary>
/// Método para obter o nome do jogador.
/// Pergunta ao jogador e retorna o nome.
/// </summary>
static string ObterNomeJogador()
{
    Console.WriteLine("\nQual é o seu nome?");
    string nomeJogador = Console.ReadLine();

    while (string.IsNullOrEmpty(nomeJogador))
    {
        Console.WriteLine("Você precisa digitar o seu nome. Pode ser o seu apelido...");
        nomeJogador = Console.ReadLine();
    }

    return nomeJogador;
}

///<summary>
/// Método para validar a escolha do jogador. 
/// Verifica se a escolha está dentro das opções válidas fornecidas.
///<\summary>
///<param name="opcoesValidas">Lista de opções válidas.</param>
///<returns>O caractere escolhido pelo usuário.</returns>
static int ValidaEscolha(params int[] opcoesValidas)
{
    int opcao = (int)Console.ReadKey().KeyChar - '0';
    while (!opcoesValidas.Contains(opcao))
    {
        Console.WriteLine("\nOpção inválida. Tente novamente.");
        opcao = (int)Console.ReadKey().KeyChar - '0';
    }
    return opcao;
}

/// <summary>
/// Método que determina se o jogador venceu o computador com base nas escolhas.
/// </summary>
/// <param name="jogador">Opção escolhida pelo jogador (0 = Pedra, 1 = Papel, 2 = Tesoura)</param>
/// <param name="computador">Opção escolhida pelo computador (0 = Pedra, 1 = Papel, 2 = Tesoura)</param>
/// <returns>Retorna True se o jogador vencer o computador.</returns>
static bool Vence(int jogador, int computador)
{
    return (jogador == 0 && computador == 2) ||
           (jogador == 1 && computador == 0) ||
           (jogador == 2 && computador == 1);
}

/// <summary>
/// Método que escolhe a melhor jogada do computador com base nas probabilidades.
/// </summary>
/// <param name="escolhaComputador1">Primeira opcao escolhida pelo computador</param>
/// <param name="escolhaComputador2">Segunda opcao escolhida pelo computador</param>
/// <param name="escolhaJogador1">Primeira opcao escolhida pelo jogador</param>
/// <param name="escolhaJogador2">Segunda opcao escolhida pelo jogador</param>
/// <returns>Retorna a melhor opcao do computador</returns>
static int EscolherMelhorJogadaComputador(int escolhaComputador1, int escolhaComputador2, int escolhaJogador1, int escolhaJogador2)
{
    // Matriz baseada na tabela (0 = Pedra, 1 = Papel, 2 = Tesoura)
    double[,] probabilidades = {
        { 0.25, 0.0, 0.75 }, // Pedra contra (Pedra, Papel, Tesoura)
        { 0.75, 0.25, 0.0 }, // Papel contra (Pedra, Papel, Tesoura)
        { 0.0, 0.75, 0.25 }  // Tesoura contra (Pedra, Papel, Tesoura)
    };

    // Calcula a média de vitória para cada escolha do PC contra as do jogador
    double chanceComputador1 = (probabilidades[escolhaComputador1, escolhaJogador1] + probabilidades[escolhaComputador1, escolhaJogador2]) / 2;
    double chanceComputador2 = (probabilidades[escolhaComputador2, escolhaJogador1] + probabilidades[escolhaComputador2, escolhaJogador2]) / 2;

    // Escolhe a opção com maior chance de vitória
    return (chanceComputador1 >= chanceComputador2) ? escolhaComputador1 : escolhaComputador2;
}

/// <summary>
/// Método que lista as estatísticas de todos os jogadores.
/// </summary>
void ListarEstatisticasJogadores()
{
    Console.WriteLine("\nJogadores e suas estatísticas:\n");
    Console.WriteLine("===================================================================");
    foreach (var jogador in jogadores)
    {
        Console.WriteLine($"{jogador.Key}: {jogador.Value.vitórias} vitórias, {jogador.Value.empates} empates, {jogador.Value.derrotas} derrotas");
    }
    Console.WriteLine("===================================================================\n");
}

/// <summary>
/// Método para recarregar a arma de roleta russa (simula aleatoriamente uma "bala").
/// </summary>
/// <returns>Retorna uma lista com as probabilidades de uma bala ser disparada.</returns>
static List<int> RecarregaArma()
{
    List<int> roletaRussa = new List<int> { 1, 0, 0, 0, 0, 0 };
    Random random = new Random();
    return roletaRussa.OrderBy(x => random.Next()).ToList();
}

/// <summary>
/// Método que simula uma roleta russa.
/// Se o jogador "perde", uma bala é disparada (simulada).
/// </summary>
/// <param name="nomeJogador">Pega o nome do jogador da partida</param>
/// <param name="roletaRussa">Lista que armazena os valores de 0s e 1 simulando a unica chance de 6 de morrer</param>
/// <returns>Retorna verdadeiro se o jogador "morreu", falso se sobreviveu.</returns>
static bool SimularRoletaRussa(string nomeJogador, List<int> roletaRussa)
{
    Console.WriteLine($"{nomeJogador}, você perdeu! Agora vai puxar o gatilho...");
    int indiceBala = new Random().Next(0, roletaRussa.Count);

    if (roletaRussa[indiceBala] == 1)
    {
        Console.WriteLine(FiggleFonts.Standard.Render("BANG!"));
        Console.WriteLine($" {nomeJogador} morreu!");
        return true;
    }
    else
    {
        Console.WriteLine(FiggleFonts.Standard.Render("CLIC!"));
        Console.WriteLine($" {nomeJogador} sobreviveu...");
        roletaRussa.Remove(0);
        Console.WriteLine($"A arma recebeu mais uma bala. Tem 7 slots {roletaRussa.Count} estão vazios...");
        return false;
    }
}
#endregion
