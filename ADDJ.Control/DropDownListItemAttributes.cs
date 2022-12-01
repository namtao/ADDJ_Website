using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VnptNet.Control.DropDownListItemAttributes
{
    ///<summary>
    /// inheriting the DropDownList to save attributes
    ///</summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:ServerControl1 runat=server></{0}:ServerControl1>")]
    public class DropDownListAttributes : DropDownList
    {
        ///<summary>
        /// By using this method save the attributes data in view state
        ///</summary>
        ///<returns></returns>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        protected override object SaveViewState()
        {
            object[] allStates = new object[this.Items.Count + 1];

            object baseState = base.SaveViewState();
            allStates[0] = baseState;

            Int32 i = 1;

            foreach (ListItem li in this.Items)
            {
                Int32 j = 0;
                string[][] attributes = new string[li.Attributes.Count][];
                foreach (string attribute in li.Attributes.Keys)
                    attributes[j++] = new string[] { attribute, li.Attributes[attribute] };

                allStates[i++] = attributes;
            }
            return allStates;
        }

        ///<summary>
        /// /// By using this method Load the attributes data from view state
        ///</summary>
        ///<param name="savedState"></param>
        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] myState = (object[])savedState;

                if (myState[0] != null)
                    base.LoadViewState(myState[0]);

                Int32 i = 1;
                foreach (ListItem li in this.Items)
                    foreach (string[] attribute in (string[][])myState[i++])
                        li.Attributes[attribute[0]] = attribute[1];
            }
        }
    }
}