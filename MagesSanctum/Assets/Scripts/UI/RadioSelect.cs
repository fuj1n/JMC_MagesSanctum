using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RadioSelect : MonoBehaviour, IPointerClickHandler
{
    public static RadioController Controller
    {
        get
        {
            if (!controller)
                controller = new GameObject("Radio Controller").AddComponent<RadioController>();
            return controller;
        }
    }
    private static RadioController controller;

    public string radioId;
    public bool isSelected = false;

    [Header("Display")]
    public Graphic graphic;
    public Color selectedColor = Color.white;

    private Color baseColor;

    public object additionalData;

    private void Awake()
    {
        Controller.Register(radioId, this);

        if (graphic)
            baseColor = graphic.color;
    }

    public void Select()
    {
        Controller.DeselectAll(radioId);

        isSelected = true;

        if (graphic)
            graphic.color = baseColor * selectedColor;
    }

    public void Deselect()
    {
        isSelected = false;

        if (graphic)
            graphic.color = baseColor;
    }

    public void OnPointerClick(PointerEventData eventData) => Select();

    public class RadioController : MonoBehaviour
    {
        private Dictionary<string, HashSet<RadioSelect>> radioMap = new Dictionary<string, HashSet<RadioSelect>>();

        public void Register(string type, RadioSelect button)
        {
            if (!radioMap.ContainsKey(type))
                radioMap.Add(type, new HashSet<RadioSelect>());

            if (radioMap[type] == null)
                radioMap[type] = new HashSet<RadioSelect>();

            if (radioMap[type].Any(x => x.isSelected))
                button.Deselect();

            radioMap[type].Add(button);
        }

        public void UnRegister(string type, RadioSelect button)
        {
            if (!radioMap.ContainsKey(type))
                return;

            if (radioMap[type] == null)
                return;

            radioMap[type].Remove(button);
        }

        public void DeselectAll(string type)
        {
            if (!radioMap.ContainsKey(type))
                return;

            foreach (RadioSelect r in radioMap[type])
                r.Deselect();
        }

        public RadioSelect GetSelection(string type)
        {
            if (!radioMap.ContainsKey(type))
                return null;

            return radioMap[type].Where(r => r.isSelected).FirstOrDefault();
        }

        private void FixedUpdate()
        {
            foreach (string type in radioMap.Keys)
                radioMap[type].RemoveWhere(x => !x);

            foreach (HashSet<RadioSelect> set in radioMap.Values)
            {
                if (set.Count == 0)
                    continue;

                var selecteds = set.Where(x => x.isSelected);
                if (selecteds.Count() == 0)
                    set.ElementAt(0).Select();
                else if (selecteds.Count() > 1)
                    for (int i = 1; i < selecteds.Count(); i++)
                        selecteds.ElementAt(i).Deselect();
            }
        }
    }
}
