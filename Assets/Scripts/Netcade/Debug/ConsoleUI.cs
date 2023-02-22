using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Netcade.Debug
{
    public class ConsoleUI : MonoBehaviour, Logging.GetLog
    {
        public Dictionary<int, Color32> LogColours = new Dictionary<int, Color32>()
        {
            {0, new Color32(190,190,190, 255)},
            {1, new Color32(235, 160, 70, 255)},
            {2, new Color32(255, 220, 25, 255)},
            {3, new Color32(255, 20, 40, 255)},
            {4, new Color32(10, 240, 40, 255)},
            {5, new Color32(10, 150, 240, 255)}
        };
        public Dictionary<int, string> LogNames = new Dictionary<int, string>()
        {
            {0, "Info"},
            {1, "Warning"},
            {2, "Important"},
            {3, "Error"},
            {4, "Success"},
            {5, "Networking"}
        };
    
        private void Start()
        {
            Logging.Subscribed.Add(this);
        }

        public void HideConsole()
        {
            if (Console.alpha == 1)
            {
                Console.alpha = 0;
                Console.blocksRaycasts = false;
                Console.interactable = false;
            }
            else
            {
                Console.alpha = 1;
                Console.blocksRaycasts = true;
                Console.interactable = true;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                HideConsole();
            }
        }

        public CanvasGroup Console;
        public GameObject List;
        public GameObject Template;
        public void GetLog(Logging.LogItem Item)
        {
            //Debug.Log("hello from GetLog!");
            TMP_Text inst = GameObject.Instantiate(Template, List.transform).GetComponent<TMP_Text>();
            inst.text = "<color=#" + colorToHex(LogColours[(int)Item.Type]) + ">" + Item.Frame + " " +
                        Item.Time.ToString() + " <b>" + Item.Type + "</b></color> " + Item.Text;
        }
    
        // TODO: move these to their own helper class
        public static string colorToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }
    }
}