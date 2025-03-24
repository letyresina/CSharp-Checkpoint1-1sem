using System;
using System.Collections.Generic;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Dictionary<string, (int vitórias, int empates, int derrotas)> jogadores = new Dictionary<string, (int, int, int)>();
Dictionary<int, string> jogadas = new Dictionary<int, string>
{
    { 0, "Pedra" },
    { 1, "Papel" },
    { 2, "Tesoura" }
};

int vitoriasComputador = 0, derrotasComputador = 0, empatesComputador = 0;

Console.WriteLine("Olá! Vamos jogar Jokempo Menos Um?");
Console.WriteLine("1 - Sim ou 0 - Não");
int continuar = (int)Console.ReadKey().KeyChar - '0';

while (continuar == 1)
{
    List<int> armaCarregada = RecarregaArma();
    string nomeJogador = ObterNomeJogador();

    if (!jogadores.ContainsKey(nomeJogador))
    {
        jogadores[nomeJogador] = (0, 0, 0);
    }

    continuar = 3;

    while (continuar == 3)
    {
        Console.WriteLine($"\n\n{nomeJogador} vamos jogar...");
        Console.WriteLine("O computador já fez suas escolhas.");
        Console.WriteLine("Opções: 0 - Pedra, 1 - Papel, 2 - Tesoura");

        int escolhaComputador1 = new Random().Next(0, 3);
        int escolhaComputador2 = new Random().Next(0, 3);

        Console.Write("\nEscolha sua primeira opção: ");
        int escolhaJogador1 = ValidaEscolha(0, 1, 2);

        Console.Write("\nEscolha sua segunda opção: ");
        int escolhaJogador2 = ValidaEscolha(0, 1, 2);

        Console.WriteLine($"\n\nO computador escolheu: {jogadas[escolhaComputador1]} e {jogadas[escolhaComputador2]}");
        Console.Write($"\nAgora escolha qual jogada deseja usar (1 - {jogadas[escolhaJogador1]} ou 2 - {jogadas[escolhaJogador2]}): ");
        int opcaoJogador = ValidaEscolha(1, 2);

        int jogadaFinalJogador = (opcaoJogador == 1) ? escolhaJogador1 : escolhaJogador2;
        int jogadaFinalComputador = EscolherMelhorJogadaComputador(escolhaComputador1, escolhaComputador2, escolhaJogador1, escolhaJogador2);

        Console.WriteLine($"\nVocê escolheu: {jogadas[jogadaFinalJogador]}");
        Console.WriteLine($"O computador escolheu: {jogadas[jogadaFinalComputador]}");

        if (jogadaFinalJogador == jogadaFinalComputador)
        {
            Console.WriteLine("\nEmpate!");
            jogadores[nomeJogador] = (jogadores[nomeJogador].vitórias, jogadores[nomeJogador].empates + 1, jogadores[nomeJogador].derrotas);
            empatesComputador++;
        }
        else if (Vence(jogadaFinalJogador, jogadaFinalComputador))
        {
            Console.WriteLine("\nParabéns! Você venceu!");
            jogadores[nomeJogador] = (jogadores[nomeJogador].vitórias + 1, jogadores[nomeJogador].empates, jogadores[nomeJogador].derrotas);
            derrotasComputador++;
            SimularRoletaRussa("Computador", armaCarregada) ;
        }
        else
        {
            Console.WriteLine("\nO computador venceu!");
            jogadores[nomeJogador] = (jogadores[nomeJogador].vitórias, jogadores[nomeJogador].empates, jogadores[nomeJogador].derrotas + 1);
            vitoriasComputador++;
            if (SimularRoletaRussa(nomeJogador, armaCarregada)) jogadores.Remove(nomeJogador);
        }

        Console.WriteLine($"\n1 - Jogar com outro jogador, 2 - Listar estatísticas, 3 - Continuar jogando como {nomeJogador}, 0 - Sair");
        continuar = ValidaEscolha(0, 1, 2, 3);
        if (continuar == 2)
        {
            ListarEstatisticasJogadores();
            Console.WriteLine($"\n1 - Jogar com outro jogador, 2 - Listar estatísticas, 3 - Continuar jogando como {nomeJogador}, 0 - Sair");
            continuar = ValidaEscolha(0, 1, 2, 3);
        }
        Console.Clear();
    }
}
Console.WriteLine("\nTchau! Jogamos depois!");

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

static bool Vence(int jogador, int computador)
{
    return (jogador == 0 && computador == 2) ||
           (jogador == 1 && computador == 0) ||
           (jogador == 2 && computador == 1);
}

static int EscolherMelhorJogadaComputador(int escolhaComputador1, int escolhaComputador2, int escolhaJogador1, int escolhaJogador2)
{
    if (Vence(escolhaComputador1, escolhaJogador1) || Vence(escolhaComputador1, escolhaJogador2))
    {
        return escolhaComputador1;
    }
    else if (Vence(escolhaComputador2, escolhaJogador1) || Vence(escolhaComputador2, escolhaJogador2))
    {
        return escolhaComputador2;
    }
    else
    {
        return escolhaComputador1;
    }
}

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

// Função de recarregar a arma.
static List<int> RecarregaArma()
{
    List<int> roletaRussa = new List<int> { 1, 0, 0, 0, 0, 0 };
    Random random = new Random();
    return roletaRussa.OrderBy(x => random.Next()).ToList();
}

static bool SimularRoletaRussa(string nomeJogador, List<int> roletaRussa)
{
    Console.WriteLine($"{nomeJogador}, você perdeu! Agora vai puxar o gatilho...");
    int indiceBala = new Random().Next(0, roletaRussa.Count);

    if (roletaRussa[indiceBala] == 1)
    {
        Console.WriteLine($"BANG! {nomeJogador} morreu!");
        return true;
    }
    else
    {
        Console.WriteLine($"CLIC! {nomeJogador} sobreviveu...");
        return false;
    }
}
