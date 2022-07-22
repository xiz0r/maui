using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;
using PlatformView = UIKit.UIView;

namespace Microsoft.Maui.Handlers
{
	public partial class ViewHandler
	{
#pragma warning disable CA1416 // Validate platform compatibility
		private sealed class MyUIContextMenuInteractionDelegate : NSObject, IUIContextMenuInteractionDelegate
		{
			readonly Func<UIContextMenuConfiguration> _a;

			public MyUIContextMenuInteractionDelegate(Func<UIContextMenuConfiguration> a)
			{
				_a = a;
			}
			public UIContextMenuConfiguration? GetConfigurationForMenu(UIContextMenuInteraction interaction, CGPoint location)
			{
				return _a();
			}
		}

		partial void ConnectingHandler(PlatformView? platformView)
		{
			if (VirtualView is IContextActionContainer contextActionContainer)
			{
				if (contextActionContainer.ContextActions?.Any() == true)
				{
					var uiMenuElements = new List<UIMenuElement>();
					AddMenuItems(contextActionContainer.ContextActions, uiMenuElements);
					var uiMenu = UIMenu.Create(uiMenuElements.ToArray());

					var newFlyout = new UIContextMenuInteraction(
						@delegate:
						new MyUIContextMenuInteractionDelegate(
							() =>
							{
								return UIContextMenuConfiguration.Create(
									identifier: null,
									previewProvider: null,
									actionProvider: _ => uiMenu);
							}));

					platformView!.AddInteraction(newFlyout);
				}
			}
		}

		void AddMenuItems(IList<IMenuElement> menuItems, List<UIMenuElement> destinationList)
		{
			foreach (var menuItem in menuItems)
			{
				switch (menuItem)
				{
					case IMenuFlyoutSubItem menuFlyoutSubItem:
						var uiSubMenuElements = new List<UIMenuElement>();

						AddMenuItems(menuFlyoutSubItem, uiSubMenuElements);

						var newSubMenuElement = UIMenu.Create(
							title: menuFlyoutSubItem.Text,
							image: menuFlyoutSubItem.Source?.GetPlatformMenuImage(MauiContext!),
							identifier: UIMenuIdentifier.None,
							options: 0, // show as regular menu items
							children: uiSubMenuElements.ToArray());

						destinationList.Add(newSubMenuElement);
						break;

					case IMenuFlyoutSeparator menuFlyoutSeparator:
						var newSeparator =
							UIMenu.Create(
								string.Empty,
								image: null,
								UIMenuIdentifier.None,
								UIMenuOptions.DisplayInline,
								children: Array.Empty<UIMenuElement>());

						destinationList.Add(newSeparator);
						break;

					default:
						var newMenuElement = UIAction.Create(
							title: menuItem.Text,
							image: menuItem.Source?.GetPlatformMenuImage(MauiContext!),
							identifier: null,
							handler: _ => { menuItem.Clicked(); });

						//UpdateNativeMenuItem(menuItem, newItem);

						destinationList.Add(newMenuElement);
						break;
				}
			}
		}
#pragma warning restore CA1416 // Validate platform compatibility

		static partial void MappingFrame(IViewHandler handler, IView view)
		{
			UpdateTransformation(handler, view);
			handler.ToPlatform().UpdateBackgroundLayerFrame();
		}

		public static void MapTranslationX(IViewHandler handler, IView view)
		{
			UpdateTransformation(handler, view);
		}

		public static void MapTranslationY(IViewHandler handler, IView view)
		{
			UpdateTransformation(handler, view);
		}

		public static void MapScale(IViewHandler handler, IView view)
		{
			UpdateTransformation(handler, view);
		}

		public static void MapScaleX(IViewHandler handler, IView view)
		{
			UpdateTransformation(handler, view);
		}

		public static void MapScaleY(IViewHandler handler, IView view)
		{
			UpdateTransformation(handler, view);
		}

		public static void MapRotation(IViewHandler handler, IView view)
		{
			UpdateTransformation(handler, view);
		}

		public static void MapRotationX(IViewHandler handler, IView view)
		{
			UpdateTransformation(handler, view);
		}

		public static void MapRotationY(IViewHandler handler, IView view)
		{
			UpdateTransformation(handler, view);
		}

		public static void MapAnchorX(IViewHandler handler, IView view)
		{
			UpdateTransformation(handler, view);
		}

		public static void MapAnchorY(IViewHandler handler, IView view)
		{
			UpdateTransformation(handler, view);
		}

		internal static void UpdateTransformation(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public virtual bool NeedsContainer
		{
			get
			{
				return VirtualView?.Clip != null || VirtualView?.Shadow != null || (VirtualView as IBorder)?.Border != null;
			}
		}
	}
}