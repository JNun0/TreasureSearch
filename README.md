# TreasureSearch
 
O TreasureSearch é um jogo onde há a geração de um labirinto utilizando a lógica de DFS e há o spawn de 1 tesouro, 3 agentes e do player.
O objetivo do jogo é o jogador percorrer o labirinto e chegar ao tesouro, sem entrar em contacto com os outros agentes.
Quando o jogador entra em contacto com o tesouro o jogo volta ao momento inicial.
O jogador é controlado através das setas.

Os agentes movimentam-se através da Navmesh e da lógica A* e vão todos para um local aleatório no labirinto. Chegando a esse local dão respawn num local aleatório do labirinto e voltam a deslocar-se para o mesmo local.

Quando os agentes colidem com o jogador, o jogador volta ao local de partida e o agente dá também respawn para um local aleatório.
Os agentes tem também uma máquina de estados onde no momento inicial se deslocam até ao destino e quando o jogador se aproxima, eles seguem-no e tentam colidir com o jogador, até o jogador ter uma certa distância deles, ao qual depois retomam ao seu deslocamento até ao destino.

Há também um botão de reset onde o jogo volta ao momento inicial com um novo labirinto gerado e o botão exit para sair do jogo. 