using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WS8610GUI
{
	/// <summary>
	/// This class adds to the functionality provided in System.Windows.Forms.MenuStrip.
	///
	/// It allows you to "ClickThrough" to the MenuStrip so that you don't have to click once to
	/// bring the form into focus and once more to take the desired action
	///
	/// It also implements a SuppressHighlighting property to turn off the highlighting
	/// that occures on mouseover when the form is not active
	///
	/// The ideas behind this were borrowed from two items found on the Internet:
	///
	/// Rick Brewster shows how to implement ClickThrough on his blog at:
	///   http://blogs.msdn.com/rickbrew/
	///
	/// JasonD suggests the method to suppress the highlighting on at forum at:
	///   http://forums.microsoft.com/MSDN/ShowPost.aspx?PostID=118385&SiteID=1	///
	/// </summary>
	public partial class MenuStripEx : MenuStrip
	{
		public const uint WM_MOUSEMOVE = 0x0200;
		public const uint WM_MOUSEACTIVATE = 0x21;
		public const uint MA_ACTIVATE = 1;
		public const uint MA_ACTIVATEANDEAT = 2;
		public const uint MA_NOACTIVATE = 3;
		public const uint MA_NOACTIVATEANDEAT = 4;
		private bool clickThrough = true;
		private bool suppressHighlighting = false;

		/// <summary>
		/// Gets or sets whether the control honors item clicks when its containing form does
		/// not have input focus.
		/// </summary>
		/// <remarks>
		/// Default value is false, which is the same behavior provided by the base ToolStrip class.
		/// </remarks>
		public bool ClickThrough {
			get { return this.clickThrough; }
			set { this.clickThrough = value; }
		}

		/// <summary>
		/// Gets or sets whether the control shows highlighting on mouseover
		/// </summary>
		/// <remarks>
		/// Default value is true, which is the same behavior provided by the base MenuStrip class.
		/// </remarks>
		public bool SuppressHighlighting {
			get { return this.suppressHighlighting; }
			set { this.suppressHighlighting = value; }
		}

		/// <summary>
		/// This method overrides the procedure that responds to Windows messages.
		///
		/// It intercepts the WM_MOUSEMOVE message
		/// and ignores it if SuppressHighlighting is on and the TopLevelControl does not contain the focus.
		/// Otherwise, it calls the base class procedure to handle the message.
		///
		/// It also intercepts the WM_MOUSEACTIVATE message and replaces an "Activate and Eat" result with
		/// an "Activate" result if ClickThrough is enabled.
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m) {
			// If we don't want highlighting, throw away mousemove commands
			// when the parent form or one of its children does not have the focus
			if (m.Msg == WM_MOUSEMOVE && this.suppressHighlighting && !this.TopLevelControl.ContainsFocus)
				return;
			else
				base.WndProc(ref m);

			// If we want ClickThrough, replace "Activate and Eat" with "Activate" on WM_MOUSEACTIVATE messages
			if (m.Msg == WM_MOUSEACTIVATE && this.clickThrough && m.Result == (IntPtr)MA_ACTIVATEANDEAT)
				m.Result = (IntPtr)MA_ACTIVATE;
		}
	}
}