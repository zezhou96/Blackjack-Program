using System;
using System.Linq;

namespace Blackjack_Program
{
    class Program
    {
        static void Main(string[] args)
        {
            //declare variables
            int playerCardValue, dealerCardValue;
            char ans;
            bool playerBJ = false;

            //declare array
            string[] playerCards = new string[20];
            int[] playerCardTotal = new int[20];
            string[] dealerCards = new string[20];
            int[] dealerCardTotal = new int[20];

            //title
            Console.WriteLine("\t****************************** BlackJack! ******************************");
            Console.WriteLine("\t-------------------------------------------------------------------------");

            //loop while player wants to play again
            do
            {
                //display your hand
                Console.WriteLine("\n\t********Your Hand********\n");
                playerCardValue = PlayerFirstHand(playerCards, playerCardTotal);

                //display dealer's hand
                Console.WriteLine("\n\n\t******Dealer's Hand******\n");
                dealerCardValue = DealerFirstHand(dealerCards, dealerCardTotal);

                //let player perform action if not blackjack
                if (playerCardValue == 21)
                {
                    Console.WriteLine("\nBlackjack!");

                    playerBJ = true;
                }                
                else
                {
                    playerCardValue = PlayerAction(playerCardValue, playerCards, playerCardTotal, dealerCards);
                }

                //dealer plays if player did not bust
                if (playerCardValue <= 21 && playerBJ == false)               
                {
                    //dealer play
                    dealerCardValue = DealerAction(dealerCardValue, dealerCards, dealerCardTotal);
                }

                //determine winner   
                WinLose(playerCardValue, dealerCardValue, playerBJ);                               

                //clear arrays and triggers
                Array.Clear(playerCards, 0, playerCards.Length);
                Array.Clear(playerCardTotal, 0, playerCardTotal.Length);
                Array.Clear(dealerCards, 0, dealerCards.Length);
                Array.Clear(dealerCardTotal, 0, dealerCardTotal.Length);
                playerBJ = false;

                //ask player if they want to play again
                do
                {
                   ans = GetSafeChar("\nPlay again? [Y/N] ");

                } while (ans != 'y' && ans != 'Y' && ans != 'n' && ans != 'N');               

            } while (ans == 'y' || ans == 'Y');

            Console.WriteLine("\nThanks For Playing!");
            
            Console.ReadLine();
        }

        //method to get safe char value
        static char GetSafeChar(string prompt)
        {
            char input = ' ';
            bool flag = false;

            do
            {
                try
                {
                    Console.Write(prompt);
                    input = char.Parse(Console.ReadLine());

                    flag = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (flag == false);

            return input;
        }

        //method for card values
        static string GetSafeCardDisplay(int cardValue)
        {
            string cardDisplay = " ";

            switch (cardValue)
            {           
                case 2:
                    cardDisplay = "2";
                    break;

                case 3:
                    cardDisplay = "3";
                    break;

                case 4:
                    cardDisplay = "4";
                    break;

                case 5:
                    cardDisplay = "5";
                    break;

                case 6:
                    cardDisplay = "6";
                    break;

                case 7:
                    cardDisplay = "7";
                    break;

                case 8:
                    cardDisplay = "8";
                    break;

                case 9:
                    cardDisplay = "9";
                    break;

                case 10:
                    cardDisplay = "10";
                    break;

                case 11:
                    cardDisplay = "A";
                    break;

                case 12:
                    cardDisplay = "J";
                    break;

                case 13:
                    cardDisplay = "Q";
                    break;

                case 14:
                    cardDisplay = "K";
                    break;

                default:
                    Console.WriteLine("Invalid");
                    break;
            }

            return cardDisplay;
        }

        //method for RNG
        static int RNG()
        {
            int cardValue;

            //random number generator
            Random random = new Random();

            //assign card           
            cardValue = random.Next(2, 15);

            return cardValue;
        }

        //method for player's 1st hand
        static int PlayerFirstHand(string[] playerCards, int[] playerCardTotal)
        {
            //declare variables
            int cardValue;
            int playerCardValue;

            //get player 1st card
            cardValue = RNG();

            //store 1st card face in playerCards array
            playerCards[0] = GetSafeCardDisplay(cardValue);

            //change Jack, Queen, King value to 10
            if (playerCards[0] == "J" || playerCards[0] == "Q" || playerCards[0] == "K")
            {
                cardValue = 10;
            }
            
            //add card value to playerCardTotal array
            playerCardTotal[0] = cardValue;        

            Console.Write("\t     |{0}|", playerCards[0]);

            //get player 2nd card
            cardValue = RNG();

            //store 2nd card face in playerCards array
            playerCards[1] = GetSafeCardDisplay(cardValue);

            //change Jack, Queen, King value to 10
            if (playerCards[1] == "J" || playerCards[1] == "Q" || playerCards[1] == "K")
            {
                cardValue = 10;
            }

            //add card value to playerCardTotal array
            playerCardTotal[1] = cardValue;    

            Console.WriteLine("\t|{0}|", playerCards[1]);

            //add up player hand total
            playerCardValue = playerCardTotal.Sum();

            //if player has 2 Ace's, change one of them to value 1
            if (playerCardValue > 21)
            {
                playerCardTotal[0] = 1;
                playerCardValue = playerCardTotal.Sum();
            }

            Console.WriteLine("\n\tTotal: {0}", playerCardValue);

            return playerCardValue;
        }

        //method for dealer's 1st hand
        static int DealerFirstHand(string[] dealerCards, int[] dealerCardTotal)
        {
            //declare variables
            int cardValue;
            int dealerCardValue;

            //get dealer 1st card
            cardValue = RNG();

            //store 1st card face into dealerCards array
            dealerCards[0] = GetSafeCardDisplay(cardValue);

            //change Jack, Queen, King values to 10
            if (dealerCards[0] == "J" || dealerCards[0] == "Q" || dealerCards[0] == "K")
            {
                cardValue = 10;
            }

            //store card value into dealerCardTotal array
            dealerCardTotal[0] = cardValue;
            dealerCardValue = dealerCardTotal.Sum();

            Console.Write("\t     |{0}|", dealerCards[0]);
            Console.Write("\t|?|");

            return dealerCardValue;
        }       

        //method for player actions
        static int PlayerAction(int playerCardValue, string[] playerCards, int[] playerCardTotal, string[] dealerCards)
        {
            //declare variables
            char ans;
            int cardValue, count = 2;
            bool firstAction = true, softTotal = false;
            string hardActionStatement, softActionStatement;

            do
            {
                //check if soft total is true
                for (int index = 0; index < playerCardTotal.Length; index++)
                {
                    if (playerCardTotal[index] == 11)
                    {
                        softTotal = true;
                    }
                }             

                //choose strategy guide
                if (softTotal == true)
                {
                    softActionStatement = SoftTotalStrat(dealerCards, playerCardValue);                    

                    if (softActionStatement == "You should Double!" && firstAction == false)
                    {
                        softActionStatement = "You should Hit!";
                    }

                    Console.WriteLine("\n\n{0}", softActionStatement);
                }
                else
                {
                    hardActionStatement = HardTotalStrat(dealerCards, playerCardValue);

                    if (hardActionStatement == "You should Double!" && firstAction == false)
                    {
                        hardActionStatement = "You should Hit!";
                    }

                    Console.WriteLine("\n\n{0}", hardActionStatement);
                }                

                //let player double/split only at their first action
                if (firstAction == true)
                {                   
                    do
                    {
                        ans = GetSafeChar("\n\nHit, Double or Stay? [H/D/S] ");

                    } while (ans != 'h' && ans != 'H' && ans != 's' && ans != 'S' && ans != 'd' && ans != 'D');                                       
                }               
                else
                {
                    do
                    {
                        ans = GetSafeChar("\n\nHit or Stay? [H/S] ");

                    } while (ans != 'h' && ans != 'H' && ans != 's' && ans != 'S');
                }

                //reset triggers
                firstAction = false;
                softTotal = false;           

                //if player chooses hit or double
                if (ans == 'h' || ans == 'H' || ans == 'd' || ans == 'D')
                {
                    //get player card
                    cardValue = RNG();

                    //get card display
                    playerCards[count] = GetSafeCardDisplay(cardValue);

                    //change Jack, Queen, King values to 10
                    if (playerCards[count] == "J" || playerCards[count] == "Q" || playerCards[count] == "K")
                    {
                        cardValue = 10;
                    }

                    //add card value to player hand value
                    playerCardTotal[count] = cardValue;

                    //sum of hand
                    playerCardValue = playerCardTotal.Sum();

                    //if player hand value goes over 21
                    if (playerCardValue > 21)
                    {
                        //search array if there's Ace, if so, -10 from playerCardValue for each Ace
                        for (int index = 0; index < playerCardTotal.Length; index++)
                        {
                            if (playerCardTotal[index] == 11)
                            {
                                playerCardTotal[index] = 1;

                                playerCardValue = playerCardTotal.Sum();
                            }
                        }                        
                    }                 

                    Console.WriteLine("\n     |{0}|", playerCards[count]);
                    Console.WriteLine("\nTotal: {0}",playerCardValue);

                    if (playerCardValue > 21)
                    {
                        Console.WriteLine("\nBust!");
                    }
                    else if (playerCardValue == 21)
                    {
                        Console.WriteLine("\n21!");
                    }

                    count += 1;
                }                

            } while ((ans == 'H' || ans == 'h') && playerCardValue < 21);

            return playerCardValue;
        }
     
        //method to let dealer play
        static int DealerAction(int dealerCardValue, string[] dealerCards, int[] dealerCardTotal)
        {
            //declare variables
            int cardValue, count = 1;
            bool soft17;

            Console.WriteLine("\n*******Dealer's draw*******");

            do
            {
                //get dealer cards
                cardValue = RNG();

                //get card display for dealer hand
                dealerCards[count] = GetSafeCardDisplay(cardValue);

                //change Jack, Queen, King values to 10
                if (dealerCards[count] == "J" || dealerCards[count] == "Q" || dealerCards[count] == "K")
                {
                    cardValue = 10;
                }

                //add card value to dealer hand value
                dealerCardTotal[count] = cardValue;

                //sum of hand
                dealerCardValue = dealerCardTotal.Sum();

                //make soft17 = false by default so it doesn't loop
                soft17 = false;

                //check for soft 17
                if (dealerCardValue == 17)
                {
                    for (int index = 0; index < dealerCardTotal.Length; index++)
                    {
                        if (dealerCardTotal[index] == 11)
                        {
                            soft17 = true;
                        }
                    }
                }

                //if dealer hand value goes over 21
                if (dealerCardValue > 21)
                {
                    //search array if there's Ace, if so, -10 from dealerCardValue for each Ace
                    for (int index = 0; index < dealerCardTotal.Length; index++)
                    {
                        if (dealerCardTotal[index] == 11)
                        {
                            dealerCardTotal[index] -= 10;

                            dealerCardValue = dealerCardTotal.Sum();
                        }
                       
                    }
                }              

                Console.WriteLine("\n\t     |{0}|", dealerCards[count]);

                count += 1;

            } while (dealerCardValue <17 || soft17 == true);

            //print results
            if (dealerCardValue >= 17 && dealerCardValue <= 21)
            {
                Console.WriteLine("\nDealer's Total: {0}", dealerCardValue);
            }
            else if (dealerCardValue > 21)
            {
                Console.WriteLine("\nDealer's Total: {0}", dealerCardValue);
                Console.WriteLine("Dealer Bust!");
            }

            return dealerCardValue;
        }

        //method for winner/loser
        static void WinLose(int playerCardValue, int dealerCardValue, bool playerBJ)
        {
            if (playerBJ == true || dealerCardValue > 21 || playerCardValue > dealerCardValue && playerCardValue <= 21)
            {
                Console.WriteLine("You Win!");
            }
            else if (playerCardValue > 21 || dealerCardValue > playerCardValue && dealerCardValue <= 21)
            {
                Console.WriteLine("Dealer Wins!");
            }
            else
            {
                Console.WriteLine("Push!");
            }
        }

        //method for Hard Total strategy
        static string HardTotalStrat(string[] dealerCards, int playerCardValue)
        {
            char action = ' ';
            int dealerColumn = 0;
            string actionStatement = " ";

            //2D array for strategy guide
            char[,] hardTotalStratGuide = new char[10, 10]
            {
               // 2   3   4   5   6   7   8   9  10   A
                {'S','S','S','S','S','S','S','S','S','S'},  //17
                {'S','S','S','S','S','H','H','H','H','H'},  //16
                {'S','S','S','S','S','H','H','H','H','H'},  //15
                {'S','S','S','S','S','H','H','H','H','H'},  //14
                {'S','S','S','S','S','H','H','H','H','H'},  //13
                {'H','H','S','S','S','H','H','H','H','H'},  //12
                {'D','D','D','D','D','D','D','D','D','D'},  //11
                {'D','D','D','D','D','D','D','D','H','H'},  //10
                {'H','D','D','D','D','H','H','H','H','H'},  //9
                {'H','H','H','H','H','H','H','H','H','H'}   //8
            };

            //assign strat guide column for showing dealer card
            switch (dealerCards[0])
            {
                case "2":
                    dealerColumn = 0;
                    break;

                case "3":
                    dealerColumn = 1;
                    break;

                case "4":
                    dealerColumn = 2;
                    break;

                case "5":
                    dealerColumn = 3;
                    break;

                case "6":
                    dealerColumn = 4;
                    break;

                case "7":
                    dealerColumn = 5;
                    break;

                case "8":
                    dealerColumn = 6;
                    break;

                case "9":
                    dealerColumn = 7;
                    break;

                case "10":
                case "J":
                case "Q":
                case "K":
                    dealerColumn = 8;
                    break;

                case "A":
                    dealerColumn = 9;
                    break;

                default:
                    Console.WriteLine("Invalid");
                    break;
            }

            //find action
            switch (playerCardValue)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    action = hardTotalStratGuide[9,dealerColumn];
                    break;

                case 9:
                    action = hardTotalStratGuide[8, dealerColumn];
                    break;

                case 10:
                    action = hardTotalStratGuide[7, dealerColumn];
                    break;

                case 11:
                    action = hardTotalStratGuide[6, dealerColumn];
                    break;

                case 12:
                    action = hardTotalStratGuide[5, dealerColumn];
                    break;

                case 13:
                    action = hardTotalStratGuide[4, dealerColumn];
                    break;

                case 14:
                    action = hardTotalStratGuide[3, dealerColumn];
                    break;

                case 15:
                    action = hardTotalStratGuide[2, dealerColumn];
                    break;

                case 16:
                    action = hardTotalStratGuide[1, dealerColumn];
                    break;

                case 17:
                case 18:
                case 19:
                case 20:
                    action = hardTotalStratGuide[0, dealerColumn];
                    break;

                default:
                    Console.WriteLine("Invalid");
                    break;
            }

            //print action
            switch (action)
            {
                case 'S':
                    actionStatement = "You should Stay!";
                    break;

                case 'H':
                    actionStatement = "You should Hit!";
                    break;

                case 'D':
                    actionStatement = "You should Double!";
                    break;

                default:
                    Console.WriteLine("Invalid");
                    break;
            }

            return actionStatement;

        }

        //method for soft total strategy
        static string SoftTotalStrat(string[] dealerCards, int playerCardValue)
        {
            int dealerColumn = 0;
            char action = ' ';
            string actionStatement = " ";

            char[,] softTotalStratGuide = new char[8, 10]
            {
               // 2   3   4   5   6   7   8   9  10   A
                {'S','S','S','S','S','S','S','S','S','S'},  //A,9
                {'S','S','S','S','D','S','S','S','S','S'},  //A,8
                {'D','D','D','D','D','S','S','H','H','H'},  //A,7
                {'H','D','D','D','D','H','H','H','H','H'},  //A,6
                {'H','H','D','D','D','H','H','H','H','H'},  //A,5
                {'H','H','D','D','D','H','H','H','H','H'},  //A,4
                {'H','H','H','D','D','H','H','H','H','H'},  //A,3
                {'H','H','H','D','D','H','H','H','H','H'},  //A,2
            };

            //assign strat guide column for showing dealer card
            switch (dealerCards[0])
            {
                case "2":
                    dealerColumn = 0;
                    break;

                case "3":
                    dealerColumn = 1;
                    break;

                case "4":
                    dealerColumn = 2;
                    break;

                case "5":
                    dealerColumn = 3;
                    break;

                case "6":
                    dealerColumn = 4;
                    break;

                case "7":
                    dealerColumn = 5;
                    break;

                case "8":
                    dealerColumn = 6;
                    break;

                case "9":
                    dealerColumn = 7;
                    break;

                case "10":
                case "J":
                case "Q":
                case "K":
                    dealerColumn = 8;
                    break;

                case "A":
                    dealerColumn = 9;
                    break;

                default:
                    Console.WriteLine("Invalid");
                    break;
            }

            //find action
            switch (playerCardValue)
            {
                case 13:
                    action = softTotalStratGuide[7, dealerColumn];
                    break;

                case 14:
                    action = softTotalStratGuide[6, dealerColumn];
                    break;

                case 15:
                    action = softTotalStratGuide[5, dealerColumn];
                    break;

                case 16:
                    action = softTotalStratGuide[4, dealerColumn];
                    break;

                case 17:
                    action = softTotalStratGuide[3, dealerColumn];
                    break;

                case 18:
                    action = softTotalStratGuide[2, dealerColumn];
                    break;

                case 19:
                    action = softTotalStratGuide[1, dealerColumn];
                    break;

                case 20:
                    action = softTotalStratGuide[0, dealerColumn];
                    break;

                default:
                    Console.WriteLine("Invalid");
                    break;
            }

            //print action
            switch (action)
            {
                case 'S':
                    actionStatement = "You should Stay!";
                    break;

                case 'H':
                    actionStatement = "You should Hit!";
                    break;

                case 'D':
                    actionStatement = "You should Double!";
                    break;

                default:
                    Console.WriteLine("Invalid");
                    break;
            }

            return actionStatement;

        }
    }
}
