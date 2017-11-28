﻿using System.Collections.Generic;
using Intersect;
using Intersect.Client.Classes.UI.Game.Inventory;
using Intersect.GameObjects;
using Intersect.Client.Classes.Localization;
using IntersectClientExtras.GenericClasses;
using IntersectClientExtras.Gwen.Control;
using Intersect_Client.Classes.General;
using Point = IntersectClientExtras.GenericClasses.Point;

namespace Intersect_Client.Classes.UI.Game
{
    public class InventoryWindow
    {
        //Initialized Items?
        private bool mInitializedItems = false;

        //Controls
        private WindowControl mInventoryWindow;

        private ScrollControl mItemContainer;
        private List<Label> mValues = new List<Label>();

        //Item List
        public List<InventoryItem> Items = new List<InventoryItem>();

        //Init
        public InventoryWindow(Canvas gameCanvas)
        {
            mInventoryWindow = new WindowControl(gameCanvas, Strings.Inventory.title, false,
                "InventoryWindow");
            mInventoryWindow.DisableResizing();

            mItemContainer = new ScrollControl(mInventoryWindow, "ItemsContainer");
            mItemContainer.EnableScroll(false, true);
        }

        //Location
        //Location
        public int X
        {
            get { return mInventoryWindow.X; }
        }

        public int Y
        {
            get { return mInventoryWindow.Y; }
        }

        //Methods
        public void Update()
        {
            if (!mInitializedItems)
            {
                mInitializedItems = true;
                InitItemContainer();
            }
            if (mInventoryWindow.IsHidden == true)
            {
                return;
            }
            for (int i = 0; i < Options.MaxInvItems; i++)
            {
                var item = ItemBase.Lookup.Get<ItemBase>(Globals.Me.Inventory[i].ItemNum);
                if (item != null)
                {
                    Items[i].Pnl.IsHidden = false;
                    if (item.IsStackable())
                    {
                        mValues[i].IsHidden = false;
                        mValues[i].Text = Globals.Me.Inventory[i].ItemVal.ToString();
                    }
                    else
                    {
                        mValues[i].IsHidden = true;
                    }

                    if (Items[i].IsDragging)
                    {
                        Items[i].Pnl.IsHidden = true;
                        mValues[i].IsHidden = true;
                    }
                    Items[i].Update();
                }
                else
                {
                    Items[i].Pnl.IsHidden = true;
                    mValues[i].IsHidden = true;
                }
            }
        }

        private void InitItemContainer()
        {
            for (int i = 0; i < Options.MaxInvItems; i++)
            {
                Items.Add(new InventoryItem(this, i));
                Items[i].Container = new ImagePanel(mItemContainer, "InventoryItemContainer");
                Items[i].Setup();

                mValues.Add(new Label(Items[i].Container, "InventoryItemValue"));
                mValues[i].Text = "";

                //TODO Made this more efficient.
                Gui.LoadRootUiData(Items[i].Container, "InGame.xml");

                var xPadding = Items[i].Container.Padding.Left + Items[i].Container.Padding.Right;
                var yPadding = Items[i].Container.Padding.Top + Items[i].Container.Padding.Bottom;
                Items[i].Container.SetPosition(
                    (i % (mItemContainer.Width / (Items[i].Container.Width + xPadding))) *
                    (Items[i].Container.Width + xPadding) + xPadding,
                    (i / (mItemContainer.Width / (Items[i].Container.Width + xPadding))) *
                    (Items[i].Container.Height + yPadding) + yPadding);
            }
        }

        public void Show()
        {
            mInventoryWindow.IsHidden = false;
        }

        public bool IsVisible()
        {
            return !mInventoryWindow.IsHidden;
        }

        public void Hide()
        {
            mInventoryWindow.IsHidden = true;
        }

        public FloatRect RenderBounds()
        {
            FloatRect rect = new FloatRect()
            {
                X = mInventoryWindow.LocalPosToCanvas(new Point(0, 0)).X -
                    (Items[0].Container.Padding.Left + Items[0].Container.Padding.Right) / 2,
                Y = mInventoryWindow.LocalPosToCanvas(new Point(0, 0)).Y -
                    (Items[0].Container.Padding.Top + Items[0].Container.Padding.Bottom) / 2,
                Width = mInventoryWindow.Width + (Items[0].Container.Padding.Left + Items[0].Container.Padding.Right),
                Height = mInventoryWindow.Height + (Items[0].Container.Padding.Top + Items[0].Container.Padding.Bottom)
            };
            return rect;
        }
    }
}