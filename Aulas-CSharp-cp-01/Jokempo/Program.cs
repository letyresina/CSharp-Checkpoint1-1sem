using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Dicionário com as opções do jogo
        Dictionary<int, string> jogadas = new Dictionary<int, string>
        {
            { 0, "Pedra" },
            { 1, "Papel" },
            { 2, "Tesoura" }
        };

        while (true)
        {
            Console.WriteLine("\n\nOlá! Vamos jogar Jokempo Menos Um?");
            Console.WriteLine("1 - Sim ou 0 - Não");
            if (Console.ReadKey().KeyChar == '0')
            {
                Console.WriteLine("\nTchau! Jogamos depois!");
                break;
            }

            Console.WriteLine("\n\nCada jogador escolhe duas opções, mas só pode usar uma para competir.");
            Console.WriteLine("Opções: 0 - Pedra, 1 - Papel, 2 - Tesoura");

            int escolhaComputador1 = new Random().Next(0, 3); // Computador escolhe aleatoriamente a primeira jogada
            int escolhaComputador2 = new Random().Next(0, 3); // Computador escolhe aleatoriamente a segunda jogada

            Console.WriteLine("\nO computador já fez suas escolhas.");

            // Escolhas do jogador
            Console.Write("\nEscolha sua primeira opção: ");
            int escolhaJogador1 = (int)(Console.ReadKey().KeyChar - '0'); 
            Console.Write("\nEscolha sua segunda opção: ");
            int escolhaJogador2 = (int)(Console.ReadKey().KeyChar - '0'); 

            Console.WriteLine($"\n\nVocê escolheu: {jogadas[escolhaJogador1]} e {jogadas[escolhaJogador2]}");
            Console.WriteLine($"O computador escolheu: {jogadas[escolhaComputador1]} e {jogadas[escolhaComputador2]}");

            // Jogador escolhe qual jogada usar
            Console.Write($"\nAgora escolha qual jogada deseja usar (1 - {jogadas[escolhaJogador1]} ou 2 - {jogadas[escolhaJogador2]}): ");
            int opcaoJogador = (int)(Console.ReadKey().KeyChar - '0'); 
            while (opcaoJogador != 1 && opcaoJogador != 2)
            {
                Console.Write("\nEscolha inválida, escolha 1 ou 2!");
                opcaoJogador = (int)(Console.ReadKey().KeyChar - '0');
            }

            int jogadaFinalJogador;

            if (opcaoJogador == 1)
            { jogadaFinalJogador = escolhaJogador1; }
            else
            { jogadaFinalJogador = escolhaJogador2; }

            // O computador escolhe sua jogada de forma estratégica
            int jogadaFinalComputador = EscolherMelhorJogadaComputador(escolhaComputador1, escolhaComputador2, escolhaJogador1, escolhaJogador2);

            // Exibe as jogadas finais escolhidas
            Console.WriteLine($"\nVocê escolheu: {jogadas[jogadaFinalJogador]}");
            Console.WriteLine($"O computador escolheu: {jogadas[jogadaFinalComputador]}");

            // Verificação do vencedor
            if (jogadaFinalJogador == jogadaFinalComputador)
            {
                Console.WriteLine("\nEmpate!");
            }
            else if (Vence(jogadaFinalJogador, jogadaFinalComputador))
            {
                Console.WriteLine("\nParabéns! Você venceu!");
            }
            else
            {
                Console.WriteLine("\nO computador venceu!");
            }

            // Pergunta se o jogador quer continuar
            Console.WriteLine("\nDeseja jogar novamente? (1 - Sim, 0 - Não)");
            if (Console.ReadKey().KeyChar == '0')
            {
                Console.WriteLine("\nTchau! Jogamos depois!");
                break;
            }
            else
            {
                Console.WriteLine("\nVamos jogar novamente!");
            }
        }
    }

    // Função para verificar quem vence
    static bool Vence(int jogador, int computador)
    {
        return (jogador == 0 && computador == 2) ||  // Pedra vence Tesoura
               (jogador == 1 && computador == 0) ||  // Papel vence Pedra
               (jogador == 2 && computador == 1);    // Tesoura vence Papel
    }

    // Função para escolher a melhor jogada do computador
    static int EscolherMelhorJogadaComputador(int escolhaComputador1, int escolhaComputador2, int escolhaJogador1, int escolhaJogador2)
    {
        // Se o computador pode vencer qualquer uma das jogadas do jogador, ele escolhe a melhor opção
        if (Vence(escolhaComputador1, escolhaJogador1) || Vence(escolhaComputador1, escolhaJogador2))
        {
            return escolhaComputador1;
        }
        else if (Vence(escolhaComputador2, escolhaJogador1) || Vence(escolhaComputador2, escolhaJogador2))
        {
            return escolhaComputador2;
        }
        // Se não puder vencer, tenta empatar com a jogada mais frequente do jogador
        else if (escolhaComputador1 == escolhaJogador1 || escolhaComputador1 == escolhaJogador2)
        {
            return escolhaComputador1;
        }
        else if (escolhaComputador2 == escolhaJogador1 || escolhaComputador2 == escolhaJogador2)
        {
            return escolhaComputador2;
        }
        // Caso o computador não consiga nem vencer nem empatar, escolhe a jogada mais forte
        else
        {
            if (Vence(escolhaComputador1, escolhaComputador2))
            {
                return escolhaComputador1;
            }
            else
            {
                return escolhaComputador2;
            }
        }
    }
}
