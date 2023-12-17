using Monopoly.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.ViewModel
{
    public class CardViewModel
    {
        public static Queue<CardModel> ChanceCards { get; set; }
        public static Queue<CardModel> CommunityCards { get; set; }
        public static CardModel CurrentCard { get; set; }

        public CardViewModel()
        {
            ChanceCards = new Queue<CardModel>();
            CommunityCards = new Queue<CardModel>();
            PopulateStackOfCards();
        }

        private void PopulateStackOfCards()
        {
            // == Populate Chance cards ==

            ChanceCards.Enqueue(new CardModel(
                "Bank pays you dividend of $50",    //Description
                "Value",                            //Effect
                50));                               //Value

            ChanceCards.Enqueue(new CardModel(
                "Advance to Go (Collect $200)",     //Description
                "Go",                               //Effect
                0));                                //Value

            ChanceCards.Enqueue(new CardModel(
                "Get Out of Jail Free",             //Description
                "JailFree",                         //Effect
                0));                                //Value

            ChanceCards.Enqueue(new CardModel(
                "Go Back 3 Spaces",                 //Description
                "Move",                             //Effect
                0,                                  //Value
                -3));                               //Move

            ChanceCards.Enqueue(new CardModel(
                "Go to Jail. Go directly to Jail, do not pass Go, do not collect $200", //Description
                "Jail",                                                                 //Effect
                0));                                                                    //Value

            ChanceCards.Enqueue(new CardModel(
                "Speeding fine $15",                //Description
                "Value",                            //Effect
                -15));                              //Value

            ChanceCards.Enqueue(new CardModel(
                "Take a trip to Railroad. If you pass Go, collect $200",    //Description
                "NextRail",                                                 //Effect
                0));                                                        //Value

            ChanceCards.Enqueue(new CardModel(
                "Your building loan matures. Collect $150",                 //Description
                "Value",                                                    //Effect
                150));                                                      //Value


            // == Populate community chest cards ==
            CommunityCards.Enqueue(new CardModel(
                "Bank error in your favor. Collect $200",   //Description
                "Value",                                    //Effect
                200));                                      //Value

            CommunityCards.Enqueue(new CardModel(
                "Doctor’s fee. Pay $50",                    //Description
                "Value",                                    //Effect
                -50));                                      //Value

            CommunityCards.Enqueue(new CardModel(
                "From sale of stock you get $50",           //Description
                "Value",                                    //Effect
                50));                                       //Value

            CommunityCards.Enqueue(new CardModel(
                "Get Out of Jail Free",                     //Description
                "JailFree",                                 //Effect
                0));                                        //Value

            CommunityCards.Enqueue(new CardModel(
                "Go to Jail. Go directly to jail, do not pass Go, do not collect $200", //Description
                "Jail",                                                                 //Effect
                0));                                                                    //Value


            CommunityCards.Enqueue(new CardModel(
                "Holiday fund matures. Receive $100",       //Description
                "Value",                                    //Effect
                100));                                      //Value

            CommunityCards.Enqueue(new CardModel(
                "Income tax refund. Collect $20",           //Description
                "Value",                                    //Effect
                20));                                       //Value

            CommunityCards.Enqueue(new CardModel(
                "Life insurance matures. Collect $100",     //Description
                "Value",                                    //Effect
                100));                                      //Value

            CommunityCards.Enqueue(new CardModel(
                "Pay hospital fees of $100",                //Description
                "Value",                                    //Effect
                -100));                                     //Value

            CommunityCards.Enqueue(new CardModel(
                "Pay school fees of $50",                   //Description
                "Value",                                    //Effect
                -50));                                      //Value

            CommunityCards.Enqueue(new CardModel(
                "Receive $25 consultancy fee",              //Description
                "Value",                                    //Effect
                25));                                       //Value

            CommunityCards.Enqueue(new CardModel(
                "You have won second prize in a beauty contest. Collect $10",   //Description
                "Value",                                                        //Effect
                10));                                                           //Value

            CommunityCards.Enqueue(new CardModel(
                "You inherit $100",                         //Description
                "Value",                                    //Effect
                100));                                      //Value
        }

        public static void FlipACard(string type)
        {
            if (type == "Chance")
            { 
                //Gets a card from the top
                CurrentCard = ChanceCards.Dequeue();
                //Put it back to the bottom
                ChanceCards.Enqueue(CurrentCard);
            }
            else
            {
                //Gets a card from the top
                CurrentCard = CommunityCards.Dequeue();
                //Put it back to the bottom
                CommunityCards.Enqueue(CurrentCard);
            }
        }

        //Methods to add and remove a card from the pile when giving to the player

        public static void AddChanceCard(string description, string effect, int value, int move = 0)
        {
            ChanceCards.Enqueue(new CardModel(description, effect, value, move));
        }

        public static void RemoveChanceCard(CardModel card)
        {
            ChanceCards = new Queue<CardModel>(ChanceCards.Where(c => c != card));
        }

        public static void AddCommunityCard(string description, string effect, int value, int move = 0)
        {
            CommunityCards.Enqueue(new CardModel(description, effect, value, move));
        }

        public static void RemoveCommunityCard(CardModel card)
        {
            CommunityCards = new Queue<CardModel>(CommunityCards.Where(c => c != card));
        }
    }
}
