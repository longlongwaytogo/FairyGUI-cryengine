﻿using System;
using FairyGUI;

namespace CryEngine.Game
{
	public class LoopListScene : DemoScene
	{
		GComponent _mainView;
		GList _list;

		public LoopListScene()
		{
			UIPackage.AddPackage("UI/LoopList");

			_mainView = UIPackage.CreateObject("LoopList", "Main").asCom;
			_mainView.MakeFullScreen();
			_mainView.AddRelation(GRoot.inst, RelationType.Size);
			AddChild(_mainView);

			_list = _mainView.GetChild("list").asList;
			_list.SetVirtualAndLoop();

			_list.itemRenderer = RenderListItem;
			_list.numItems = 5;
			_list.scrollPane.onScroll.Add(DoSpecialEffect);

			DoSpecialEffect();
		}

		void DoSpecialEffect()
		{
			//change the scale according to the distance to middle
			float midX = _list.scrollPane.posX + _list.viewWidth / 2;
			int cnt = _list.numChildren;
			for (int i = 0; i < cnt; i++)
			{
				GObject obj = _list.GetChildAt(i);
				float dist = Math.Abs(midX - obj.x - obj.width / 2);
				if (dist > obj.width) //no intersection
					obj.SetScale(1, 1);
				else
				{
					float ss = 1 + (1 - dist / obj.width) * 0.24f;
					obj.SetScale(ss, ss);
				}
			}

			_mainView.GetChild("n3").text = "" + ((_list.GetFirstChildInView() + 1) % _list.numItems);
		}

		void RenderListItem(int index, GObject obj)
		{
			GButton item = (GButton)obj;
			item.SetPivot(0.5f, 0.5f);
			item.icon = UIPackage.GetItemURL("LoopList", "n" + (index + 1));
		}
	}
}