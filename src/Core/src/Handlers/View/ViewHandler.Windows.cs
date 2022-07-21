#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;

namespace Microsoft.Maui.Handlers
{
	public partial class ViewHandler
	{
		partial void ConnectingHandler(PlatformView? platformView)
		{
			if (platformView != null)
			{
				platformView.GotFocus += OnPlatformViewGotFocus;
				platformView.LostFocus += OnPlatformViewLostFocus;

				if (platformView is UIElement uiElement)
				{
					if (VirtualView is IContextActionContainer contextActionContainer)
					{
						if (contextActionContainer.ContextActions?.Any() == true)
						{
							var newFlyout = new MenuFlyout();
							AddMenuItems(contextActionContainer.ContextActions, newFlyout.Items.Add);
							uiElement.ContextFlyout = newFlyout;
						}
					}
				}
			}
		}

		void AddMenuItems(IList<IMenuElement> menuItems, Action<MenuFlyoutItemBase> addMenuItem)
		{
			foreach (var menuItem in menuItems)
			{
				switch (menuItem)
				{
					case IMenuFlyoutSubItem menuFlyoutSubItem:
						var newSubItem = new MenuFlyoutSubItem();

						// TODO: Need this code more generic so that flyout items and other items can share code paths
						//UpdateNativeMenuItem(menuItem, newItem);
						newSubItem.Text = menuFlyoutSubItem.Text;
						AddMenuItems(menuFlyoutSubItem, newSubItem.Items.Add);

						addMenuItem(newSubItem);
						break;

					case IMenuFlyoutSeparator menuFlyoutSeparator:
						var newSeparator = new MenuFlyoutSeparator();
						addMenuItem(newSeparator);
						break;

					default:
						var newItem = new MenuFlyoutItem();
						UpdateNativeMenuItem(menuItem, newItem);
						if (menuItem is INotifyPropertyChanged npc)
						{
							// TODO: Super hack. This is so that changes to the MAUI controls are reflected in the native menu items
							// TODO: If we can move all this MenuItem code to the Controls package, we can more closely follow the pattern used in other controls
							npc.PropertyChanged += (object? sender, PropertyChangedEventArgs e) =>
							{
								var changedMenuItem = (IMenuElement)sender!;
								UpdateNativeMenuItem(changedMenuItem, newItem);
							};
						}
						newItem.Click += (_, __) => menuItem.Clicked();

						addMenuItem(newItem);
						break;
				}
			}
		}

		private void UpdateNativeMenuItem(IMenuElement source, MenuFlyoutItem destination)
		{
			// TODO: Respect these settings too, if possible
			//menuItem.Font

			destination.CharacterSpacing = source.CharacterSpacing.ToEm();
			destination.IsEnabled = source.IsEnabled;
			destination.Text = source.Text;
			destination.Icon = source.Source?.ToIconSource(MauiContext!)?.CreateIconElement();

			// TODO: How to expose this platform-specific property for Windows only? Maybe something like this: https://docs.microsoft.com/dotnet/maui/windows/platform-specifics/listview-selectionmode
			destination.KeyboardAccelerators.Add(
				new UI.Xaml.Input.KeyboardAccelerator
				{
					Modifiers = global::Windows.System.VirtualKeyModifiers.Control,
					Key = (global::Windows.System.VirtualKey)(char.ToUpperInvariant(source.Text[0])),
				});
		}

		partial void DisconnectingHandler(PlatformView platformView)
		{
			UpdateIsFocused(false);

			platformView.GotFocus -= OnPlatformViewGotFocus;
			platformView.LostFocus -= OnPlatformViewLostFocus;
		}

		static partial void MappingFrame(IViewHandler handler, IView view)
		{
			// Both Clip and Shadow depend on the Control size.
			handler.ToPlatform().UpdateClip(view);
			handler.ToPlatform().UpdateShadow(view);
		}

		public static void MapTranslationX(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapTranslationY(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapScale(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapScaleX(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapScaleY(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapRotation(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapRotationX(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapRotationY(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapAnchorX(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapAnchorY(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapToolbar(IViewHandler handler, IView view)
		{
			if (view is IToolbarElement tb)
				MapToolbar(handler, tb);
		}

		internal static void MapToolbar(IElementHandler handler, IToolbarElement toolbarElement)
		{
			_ = handler.MauiContext ?? throw new InvalidOperationException($"{nameof(handler.MauiContext)} null");

			if (toolbarElement.Toolbar != null)
			{
				var toolBar = toolbarElement.Toolbar.ToPlatform(handler.MauiContext);
				handler.MauiContext.GetNavigationRootManager().SetToolbar(toolBar);
			}
		}

		public virtual bool NeedsContainer
		{
			get
			{
				if (VirtualView is IBorderView border)
					return border?.Shape != null || border?.Stroke != null;

				return false;
			}
		}

		void OnPlatformViewGotFocus(object sender, RoutedEventArgs args)
		{
			UpdateIsFocused(true);
		}

		void OnPlatformViewLostFocus(object sender, RoutedEventArgs args)
		{
			UpdateIsFocused(false);
		}

		void UpdateIsFocused(bool isFocused)
		{
			if (VirtualView == null)
				return;

			bool updateIsFocused = (isFocused && !VirtualView.IsFocused) || (!isFocused && VirtualView.IsFocused);

			if (updateIsFocused)
				VirtualView.IsFocused = isFocused;
		}
	}
}