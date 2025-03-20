using Figgle;

// Dicionário para armazenar estatísticas dos jogadores (nome -> (vitórias, empates, derrotas))
Dictionary<string, (int vitorias, int empates, int derrotas)> jogadores = new Dictionary<string, (int, int, int)>();

#region Métodos
/// <summary>
/// Inicializa o jogo configurando a codificação da saída do console e limpando a tela.
/// </summary>
void Inicializar()
{
    Console.OutputEncoding = System.Text.Encoding.UTF8;
    LimparTela();
}

/// <summary>
/// Limpa a tela e exibe o banner do jogo.
/// </summary>
void LimparTela()
{
    Console.BackgroundColor = ConsoleColor.Blue;
    Console.ForegroundColor = ConsoleColor.White;
    Console.Clear();
    ExibirBanner("Jokempo");
}

/// <summary>
/// Limpa a linha atual no console.
/// </summary>
void LimparLinha()
{
    Console.WriteLine("");
    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
    Console.BackgroundColor = ConsoleColor.Blue;
    Console.ForegroundColor = ConsoleColor.White;
}

/// <summary>
/// Exibe uma mensagem centralizada no console.
/// </summary>
/// <param name="mensagem">Mensagem a ser exibida.</param>
void EscreverMensagem(string mensagem)
{
    Console.SetCursorPosition((Console.WindowWidth - mensagem.Length) / 2, Console.CursorTop);
    Console.WriteLine(mensagem);
}

/// <summary>
/// Exibe um banner estilizado com a mensagem fornecida.
/// </summary>
/// <param name="mensagem">Texto do banner.</param>
void ExibirBanner(string mensagem)
{
    string banner = FiggleFonts.Larry3d.Render(mensagem);
    var linhas = banner.Split("\n");
    foreach (var linha in linhas)
    {
        if ((Console.WindowWidth - linha.Length) / 2 >= 0)
            Console.SetCursorPosition((Console.WindowWidth - linha.Length) / 2, Console.CursorTop);
        Console.WriteLine(linha);
    }
}

/// <summary>
/// Exibe um menu com opções e retorna a escolha do jogador.
/// </summary>
/// <param name="mensagem">Mensagem a ser exibida.</param>
/// <param name="opcoes">Lista de opções disponíveis.</param>
/// <returns>O caractere correspondente à escolha do usuário.</returns>
char ExibirMenu(string mensagem, params (char valor, string texto, ConsoleColor cor)[] opcoes)
{
    EscreverMensagem(mensagem);
    Console.SetCursorPosition((Console.WindowWidth - (mensagem.Length / 2)) / 2, Console.CursorTop);
    List<char> valores = new();
    List<string> botoes = new();

    foreach (var opcao in opcoes)
    {
        botoes.Add($" [{opcao.valor}] {opcao.texto} ");
        valores.Add(opcao.valor);
    }

    Console.SetCursorPosition((Console.WindowWidth - (string.Join("", botoes).Length)) / 2, Console.CursorTop);

    for (int i = 0; i < botoes.Count; i++)
    {
        Console.BackgroundColor = opcoes[i].cor;
        Console.Write(botoes[i]);
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.Write(" ");
    }

    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop + 1);
    return ValidarEntrada(valores.ToArray());
}

/// <summary>
/// Valida a entrada do usuário, garantindo que ele escolha uma opção válida.
/// </summary>
/// <param name="opcoesValidas">Lista de opções válidas.</param>
/// <returns>O caractere escolhido pelo usuário.</returns>
char ValidarEntrada(params char[] opcoesValidas)
{
    char opcao = Console.ReadKey().KeyChar;
    while (!opcoesValidas.Contains(opcao))
    {
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.ForegroundColor = ConsoleColor.Black;
        EscreverMensagem("Opção inválida. Tente novamente.");
        Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop - 1);
        opcao = Console.ReadKey().KeyChar;
    }
    return opcao;
}

/// <summary>
/// Solicita o nome do jogador e o registra no sistema.
/// </summary>
/// <returns>Nome do jogador.</returns>
string RegistrarJogador()
{
    LimparTela();
    EscreverMensagem("Qual é o seu nome?");
    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
    string nomeJogador = Console.ReadLine();

    while (string.IsNullOrEmpty(nomeJogador))
    {
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.ForegroundColor = ConsoleColor.Black;
        EscreverMensagem("Você precisa digitar o seu nome. Pode ser o seu apelido...");
        Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop - 2);
        LimparLinha();
        Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
        nomeJogador = Console.ReadLine();
    }

    if (!jogadores.ContainsKey(nomeJogador))
    {
        jogadores[nomeJogador] = (0, 0, 0);
    }

    return nomeJogador;
}

/// <summary>
/// Obtém a escolha do jogador.
/// </summary>
/// <returns>O caractere representando a escolha do jogador.</returns>
char ObterOpcaoJogador()
{
    return ExibirMenu("Escolha uma opção!", ('0', "Pedra ✊", ConsoleColor.DarkGray), ('1', "Papel ✋", ConsoleColor.DarkYellow), ('2', "Tesoura ✌", ConsoleColor.Black));
}

/// <summary>
/// Retorna o nome correspondente à opção escolhida.
/// </summary>
/// <param name="opcao">Opção escolhida pelo jogador.</param>
/// <returns>Nome da opção.</returns>
string ObterNomeOpcao(char opcao)
{
    return opcao switch
    {
        '0' => "Pedra ✊",
        '1' => "Papel ✋",
        '2' => "Tesoura ✌",
        _ => ""
    };
}

/// <summary>
/// Executa uma rodada do jogo Jokempo.
/// O jogador escolhe uma opção, o computador escolhe aleatoriamente outra, e o resultado é avaliado.
/// </summary>
/// <param name="opcao">Opção escolhida pelo jogador (0 = Pedra, 1 = Papel, 2 = Tesoura)</param>
/// <returns>Retorna 1 se o jogador venceu, 0 se houve empate e -1 se o computador venceu.</returns>
int JogarRodada(char opcao)
{
    int opcaoPC = new Random().Next(3);

    LimparTela();

    EscreverMensagem($"Você escolheu {ObterNomeOpcao(opcao)} e Eu escolhi {ObterNomeOpcao(opcaoPC.ToString()[0])}.");

    int resultado = ValidaRodada(opcao, opcaoPC.ToString()[0]);
    Console.WriteLine("");
    Console.BackgroundColor = ConsoleColor.DarkMagenta;
    switch (resultado)
    {
        case -1:
            EscreverMensagem("Haha, eu venci! Não foi dessa vez.");
            break;
        case 0:
            EscreverMensagem("Legal! Nós empatamos!");
            break;
        case 1:
            EscreverMensagem("Parabéns! Você venceu.");
            break;
    }
    LimparLinha();
    Console.WriteLine("");
    return resultado;
}

/// <summary>
/// Avalia o resultado de uma rodada com base nas escolhas do jogador e do computador.
/// </summary>
/// <param name="opcaoJogador">Opção escolhida pelo jogador.</param>
/// <param name="opcaoPC">Opção escolhida pelo computador.</param>
/// <returns>Retorna 1 se o jogador venceu, 0 se houve empate e -1 se o computador venceu.</returns>
int ValidaRodada(char opcaoJogador, char opcaoPC)
{
    int resultado = opcaoJogador switch
    {
        char o when o == opcaoPC => 0,
        char o when o == '0' && opcaoPC == '2' => 1,
        char o when o == '1' && opcaoPC == '0' => 1,
        char o when o == '2' && opcaoPC == '1' => 1,
        _ => -1
    };

    return resultado;
}

/// <summary>
/// Atualiza as estatísticas do jogador com base no resultado da rodada.
/// </summary>
/// <param name="nomeJogador">Nome do jogador.</param>
/// <param name="resultado">Resultado da rodada (-1 = derrota, 0 = empate, 1 = vitória).</param>
void AtualizarEstatisticas(string nomeJogador, int resultado)
{
    switch (resultado)
    {
        case -1:
            jogadores[nomeJogador] = (jogadores[nomeJogador].vitorias, jogadores[nomeJogador].empates, jogadores[nomeJogador].derrotas + 1);
            break;
        case 0:
            jogadores[nomeJogador] = (jogadores[nomeJogador].vitorias, jogadores[nomeJogador].empates + 1, jogadores[nomeJogador].derrotas);
            break;
        case 1:
            jogadores[nomeJogador] = (jogadores[nomeJogador].vitorias + 1, jogadores[nomeJogador].empates, jogadores[nomeJogador].derrotas);
            break;
    }
}

/// <summary>
/// Exibe a lista de jogadores cadastrados e suas estatísticas.
/// </summary>
/// <param name="continuar">Referência para a variável que armazena a decisão do jogador sobre continuar ou não.</param>
void ListarEstatisticasJogadores(ref char continuar)
{
    LimparTela();
    Console.BackgroundColor = ConsoleColor.DarkGray;
    EscreverMensagem("Jogadores e suas estatísticas:");

    LimparLinha();

    Console.WriteLine("");

    EscreverMensagem("_____________________________________________________________");
    EscreverMensagem("|       Jogador       |  Vitórias  |  Empates  |  Derrotas  |");
    foreach (var jogador in jogadores)
    {
        EscreverMensagem($"|  {jogador.Key.PadRight(19)}|  {jogador.Value.vitorias.ToString().PadRight(10)}|  {jogador.Value.empates.ToString().PadRight(9)}|  {jogador.Value.derrotas.ToString().PadRight(10)}|");
    }
    EscreverMensagem("_____________________________________________________________");

    Console.WriteLine("");
    continuar = ExibirMenu("E agora? Quer iniciar uma nova partida?", ('1', "Sim", ConsoleColor.DarkBlue), ('0', "Não", ConsoleColor.Red));
}
#endregion


#region Fluxo Principal
Inicializar();

var continuar = ExibirMenu("😀 Olá! Vamos jogar Jokempo?", ('1', "Sim", ConsoleColor.DarkBlue), ('0', "Não", ConsoleColor.Red));

while (continuar != '0')
{
    string nomeJogador = RegistrarJogador();
    LimparTela();
    EscreverMensagem($"Bem-vindo, {nomeJogador}! Vamos começar...");

    bool jogarNovamente;
    do
    {
        char opcao = ObterOpcaoJogador();
        int resultado = JogarRodada(opcao);
        AtualizarEstatisticas(nomeJogador, resultado);
        jogarNovamente = ExibirMenu("Quer jogar de novo?", ('1', "Sim", ConsoleColor.DarkBlue), ('0', "Não", ConsoleColor.Red)) == '1';
        LimparTela();
    } while (jogarNovamente);

    LimparTela();
    continuar = ExibirMenu("O que deseja fazer agora?", ('1', "Continuar com outro jogador", ConsoleColor.DarkBlue), ('2', "Listar jogadores e estatísticas", ConsoleColor.DarkGray), ('0', "Sair", ConsoleColor.Red));

    if (continuar == '2')
    {
        ListarEstatisticasJogadores(ref continuar);
    }
}

LimparTela();
EscreverMensagem("👋 Tchau! Até a próxima");
#endregion