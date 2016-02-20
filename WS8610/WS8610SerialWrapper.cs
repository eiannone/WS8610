using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace WS8610
{
	#region Enums & Structs
	public enum PortStatus { Open, Closed, Opening, Closing }

	enum DtrControl {
		Disable = 0,
		Enable = 1,
		Handshake = 2
	}

	enum RtsControl {
		Disable = 0,
		Enable = 1,
		Handshake = 2,
		Toggle = 3
	}

	[Flags]
	enum EFileAccess : uint {
		GenericRead = 0x80000000,
		GenericWrite = 0x40000000,
		GenericExecute = 0x20000000,
		GenericAll = 0x10000000
	}

	[Flags]
	enum EFileShare : uint {
		/// <summary>
		///
		/// </summary>
		None = 0x00000000,
		/// <summary>
		/// Enables subsequent open operations on an object to request read access.
		/// Otherwise, other processes cannot open the object if they request read access.
		/// If this flag is not specified, but the object has been opened for read access, the function fails.
		/// </summary>
		Read = 0x00000001,
		/// <summary>
		/// Enables subsequent open operations on an object to request write access.
		/// Otherwise, other processes cannot open the object if they request write access.
		/// If this flag is not specified, but the object has been opened for write access, the function fails.
		/// </summary>
		Write = 0x00000002,
		/// <summary>
		/// Enables subsequent open operations on an object to request delete access.
		/// Otherwise, other processes cannot open the object if they request delete access.
		/// If this flag is not specified, but the object has been opened for delete access, the function fails.
		/// </summary>
		Delete = 0x00000004
	}

	enum ECreationDisposition : uint {
		/// <summary>
		/// Creates a new file. The function fails if a specified file exists.
		/// </summary>
		New = 1,
		/// <summary>
		/// Creates a new file, always.
		/// If a file exists, the function overwrites the file, clears the existing attributes, combines the specified file attributes,
		/// and flags with FILE_ATTRIBUTE_ARCHIVE, but does not set the security descriptor that the SECURITY_ATTRIBUTES structure specifies.
		/// </summary>
		CreateAlways = 2,
		/// <summary>
		/// Opens a file. The function fails if the file does not exist.
		/// </summary>
		OpenExisting = 3,
		/// <summary>
		/// Opens a file, always.
		/// If a file does not exist, the function creates a file as if dwCreationDisposition is CREATE_NEW.
		/// </summary>
		OpenAlways = 4,
		/// <summary>
		/// Opens a file and truncates it so that its size is 0 (zero) bytes. The function fails if the file does not exist.
		/// The calling process must open the file with the GENERIC_WRITE access right.
		/// </summary>
		TruncateExisting = 5
	}

	[Flags]
	enum EFileAttributes : uint {
		Readonly = 0x00000001,
		Hidden = 0x00000002,
		System = 0x00000004,
		Directory = 0x00000010,
		Archive = 0x00000020,
		Device = 0x00000040,
		Normal = 0x00000080,
		Temporary = 0x00000100,
		SparseFile = 0x00000200,
		ReparsePoint = 0x00000400,
		Compressed = 0x00000800,
		Offline = 0x00001000,
		NotContentIndexed = 0x00002000,
		Encrypted = 0x00004000,
		Write_Through = 0x80000000,
		Overlapped = 0x40000000,
		NoBuffering = 0x20000000,
		RandomAccess = 0x10000000,
		SequentialScan = 0x08000000,
		DeleteOnClose = 0x04000000,
		BackupSemantics = 0x02000000,
		PosixSemantics = 0x01000000,
		OpenReparsePoint = 0x00200000,
		OpenNoRecall = 0x00100000,
		FirstPipeInstance = 0x00080000
	}

	[Flags]
	enum EIOControlCode : uint {
		SERIAL_SET_QUEUE_SIZE = 0x001b0008,
		SERIAL_SET_DTR = 0x001b0024,
		SERIAL_CLR_DTR = 0x001b0028,
		SERIAL_SET_RTS = 0x001b0030,
		SERIAL_CLR_RTS = 0x001b0034,
		SERIAL_GET_MODEMSTATUS = 0x001b0068
	}

	[StructLayout(LayoutKind.Sequential)]
	struct SERIAL_QUEUE_SIZE {
		public uint InSize;
		public uint OutSize;
	}

	struct Dcb {
		internal uint DCBLength;
		internal uint BaudRate;
		private BitVector32 Flags;

		//I've missed some members...
		private ushort wReserved;        // not currently used
		internal ushort XonLim;           // transmit XON threshold
		internal ushort XoffLim;          // transmit XOFF threshold             

		internal byte ByteSize;
		internal byte Parity;
		internal byte StopBits;

		//...and some more
		internal sbyte XonChar;          // Tx and Rx XON character
		internal sbyte XoffChar;         // Tx and Rx XOFF character
		internal sbyte ErrorChar;        // error replacement character
		internal sbyte EofChar;          // end of input character
		internal sbyte EvtChar;          // received event character
		private ushort wReserved1;       // reserved; do not use     

		private static readonly int fBinary;
		private static readonly int fParity;
		private static readonly int fOutxCtsFlow;
		private static readonly int fOutxDsrFlow;
		private static readonly BitVector32.Section fDtrControl;
		private static readonly int fDsrSensitivity;
		private static readonly int fTXContinueOnXoff;
		private static readonly int fOutX;
		private static readonly int fInX;
		private static readonly int fErrorChar;
		private static readonly int fNull;
		private static readonly BitVector32.Section fRtsControl;
		private static readonly int fAbortOnError;

		static Dcb() {
			// Create Boolean Mask
			fBinary = BitVector32.CreateMask();
			fParity = BitVector32.CreateMask(fBinary);
			fOutxCtsFlow = BitVector32.CreateMask(fParity);
			fOutxDsrFlow = BitVector32.CreateMask(fOutxCtsFlow);
			var previousMask = BitVector32.CreateMask(fOutxDsrFlow);
			previousMask = BitVector32.CreateMask(previousMask);
			fDsrSensitivity = BitVector32.CreateMask(previousMask);
			fTXContinueOnXoff = BitVector32.CreateMask(fDsrSensitivity);
			fOutX = BitVector32.CreateMask(fTXContinueOnXoff);
			fInX = BitVector32.CreateMask(fOutX);
			fErrorChar = BitVector32.CreateMask(fInX);
			fNull = BitVector32.CreateMask(fErrorChar);
			previousMask = BitVector32.CreateMask(fNull);
			previousMask = BitVector32.CreateMask(previousMask);
			fAbortOnError = BitVector32.CreateMask(previousMask);

			// Create section Mask
			var previousSection = BitVector32.CreateSection(1);
			previousSection = BitVector32.CreateSection(1, previousSection);
			previousSection = BitVector32.CreateSection(1, previousSection);
			previousSection = BitVector32.CreateSection(1, previousSection);
			fDtrControl = BitVector32.CreateSection(2, previousSection);
			previousSection = BitVector32.CreateSection(1, fDtrControl);
			previousSection = BitVector32.CreateSection(1, previousSection);
			previousSection = BitVector32.CreateSection(1, previousSection);
			previousSection = BitVector32.CreateSection(1, previousSection);
			previousSection = BitVector32.CreateSection(1, previousSection);
			previousSection = BitVector32.CreateSection(1, previousSection);
			fRtsControl = BitVector32.CreateSection(3, previousSection);
			previousSection = BitVector32.CreateSection(1, fRtsControl);
		}

		public bool Binary {
			get { return Flags[fBinary]; }
			set { Flags[fBinary] = value; }
		}

		public bool CheckParity {
			get { return Flags[fParity]; }
			set { Flags[fParity] = value; }
		}

		public bool OutxCtsFlow {
			get { return Flags[fOutxCtsFlow]; }
			set { Flags[fOutxCtsFlow] = value; }
		}

		public bool OutxDsrFlow {
			get { return Flags[fOutxDsrFlow]; }
			set { Flags[fOutxDsrFlow] = value; }
		}

		public DtrControl DtrControl {
			get { return (DtrControl)Flags[fDtrControl]; }
			set { Flags[fDtrControl] = (int)value; }
		}

		public bool DsrSensitivity {
			get { return Flags[fDsrSensitivity]; }
			set { Flags[fDsrSensitivity] = value; }
		}

		public bool TxContinueOnXoff {
			get { return Flags[fTXContinueOnXoff]; }
			set { Flags[fTXContinueOnXoff] = value; }
		}

		public bool OutX {
			get { return Flags[fOutX]; }
			set { Flags[fOutX] = value; }
		}

		public bool InX {
			get { return Flags[fInX]; }
			set { Flags[fInX] = value; }
		}

		public bool ReplaceErrorChar {
			get { return Flags[fErrorChar]; }
			set { Flags[fErrorChar] = value; }
		}

		public bool Null {
			get { return Flags[fNull]; }
			set { Flags[fNull] = value; }
		}

		public RtsControl RtsControl {
			get { return (RtsControl)Flags[fRtsControl]; }
			set { Flags[fRtsControl] = (int)value; }
		}

		public bool AbortOnError {
			get { return Flags[fAbortOnError]; }
			set { Flags[fAbortOnError] = value; }
		}

	}

	struct Commtimeouts {
		public UInt32 ReadIntervalTimeout;
		public UInt32 ReadTotalTimeoutMultiplier;
		public UInt32 ReadTotalTimeoutConstant;
		public UInt32 WriteTotalTimeoutMultiplier;
		public UInt32 WriteTotalTimeoutConstant;
	}
	#endregion

	internal class WS8610SerialWrapper
	{
		private const int SERIAL_BUFFER_SIZE = 16384;
		private const uint SERIAL_CTS_STATE = 0x00000010;
		private const uint SERIAL_DSR_STATE = 0x00000020;
		private SafeFileHandle _fh;
		private readonly BackgroundWorker _dataFiller;

		public string PortName { get; }

		public PortStatus Status { get; private set; }

		public WS8610SerialWrapper(string portname) {
			PortName = portname;
			_dataFiller = new BackgroundWorker();
			_dataFiller.DoWork += data_filler_DoWork;
			_dataFiller.RunWorkerCompleted += data_filler_RunWorkerCompleted;
			_dataFiller.WorkerSupportsCancellation = true;
			Status = PortStatus.Closed;
		}

		public bool Open() {
			var timeout = 50;
			while ((Status == PortStatus.Closing || Status == PortStatus.Opening) && timeout-- > 0) Thread.Sleep(100);
			if (timeout <= 0) throw new Exception("Port is busy, cannot open it.");

			if (Status == PortStatus.Open) return true;

			Status = PortStatus.Opening;

			// Setup serial port
			_fh = new SafeFileHandle(CreateFile(
				PortName,
				EFileAccess.GenericRead | EFileAccess.GenericWrite,
				EFileShare.None,    // must be opened with exclusive-access
				IntPtr.Zero, // no security attributes
				ECreationDisposition.OpenExisting, // must use OPEN_EXISTING
				EFileAttributes.Overlapped,    // not overlapped I/O
				IntPtr.Zero  // hTemplate must be NULL for comm devices
			), true);

			// Is bad handle? INVALID_HANDLE_VALUE
			if (_fh.IsInvalid) {
				/* ask the framework to marshall the win32 error code to an exception */
				Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
			}

			var queueSize = new SERIAL_QUEUE_SIZE { InSize = SERIAL_BUFFER_SIZE, OutSize = SERIAL_BUFFER_SIZE };
			uint outbuffer, bytesReturned = 0;
			DeviceIoControl(_fh, EIOControlCode.SERIAL_SET_QUEUE_SIZE, queueSize, (uint)Marshal.SizeOf(queueSize), out outbuffer, 0, ref bytesReturned, IntPtr.Zero);

			var dcb = new Dcb();
			if (!GetCommState(_fh, ref dcb)) throw new Exception("Unable to GetCommState");

			dcb.BaudRate = 300;
			dcb.Binary = false;
			dcb.Parity = 0; // NOPARITY;
			dcb.OutxCtsFlow = false;
			dcb.OutxDsrFlow = false;
			dcb.DtrControl = DtrControl.Disable;
			dcb.DsrSensitivity = false;
			dcb.TxContinueOnXoff = true;
			dcb.OutX = false;
			dcb.InX = false;
			dcb.ErrorChar = 0;
			dcb.Null = false;
			dcb.RtsControl = RtsControl.Disable;
			dcb.AbortOnError = false;
			dcb.XonLim = 0;
			dcb.XoffLim = 0;
			dcb.ByteSize = 8;
			dcb.StopBits = 0; // ONESTOPBIT;
			dcb.XonChar = 0;
			dcb.XoffChar = 0;
			dcb.ErrorChar = 0;
			dcb.EofChar = 0;
			dcb.EvtChar = 0;
			dcb.DCBLength = (uint)Marshal.SizeOf(dcb);

			if (!SetCommState(_fh, ref dcb)) {
				throw new Exception("Unable to SetCommState", Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error()));
			}

			var commtouts = new Commtimeouts {
				ReadIntervalTimeout = 0,
				ReadTotalTimeoutMultiplier = 0,
				ReadTotalTimeoutConstant = 0,
				WriteTotalTimeoutConstant = 0,
				WriteTotalTimeoutMultiplier = 0
			};
			if (!SetCommTimeouts(_fh, ref commtouts)) {
				throw new Exception("Unable to SetCommTimeouts", Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error()));
			}

			Status = _fh.IsClosed? PortStatus.Closed : PortStatus.Open;
		    if (Status != PortStatus.Open) return false;
		    _dataFiller.RunWorkerAsync();
		    return true;
		}

		public void Close() {
			if (Status == PortStatus.Closed || Status == PortStatus.Closing) return;
			Status = PortStatus.Closing;
			if (_dataFiller.IsBusy) _dataFiller.CancelAsync();
			else data_filler_RunWorkerCompleted(null, null);
		}

		public void SetDTR(bool value) {
			uint outbuffer, bytesReturned = 0;
			var cmd = value ? EIOControlCode.SERIAL_SET_DTR : EIOControlCode.SERIAL_CLR_DTR;
			DeviceIoControl(_fh, cmd, null, 0, out outbuffer, 0, ref bytesReturned, IntPtr.Zero);
		}

		public void SetRTS(bool value) {
			uint outbuffer, bytesReturned = 0;
			var cmd = value ? EIOControlCode.SERIAL_SET_RTS : EIOControlCode.SERIAL_CLR_RTS;
			DeviceIoControl(_fh, cmd, null, 0, out outbuffer, 0, ref bytesReturned, IntPtr.Zero);
		}

		public bool GetDSR() {
			uint status, bytesReturned = 0;
			DeviceIoControl(_fh, EIOControlCode.SERIAL_GET_MODEMSTATUS, null, 0, out status, sizeof(uint), ref bytesReturned, IntPtr.Zero);
			return (status & SERIAL_DSR_STATE) != 0;
		}

		public bool GetCTS() {
			uint status, bytesReturned = 0;
			DeviceIoControl(_fh, EIOControlCode.SERIAL_GET_MODEMSTATUS, null, 0, out status, sizeof(uint), ref bytesReturned, IntPtr.Zero);
			return (status & SERIAL_CTS_STATE) != 0;
		}

		void data_filler_DoWork(object sender, DoWorkEventArgs e) {
			var buffer = new byte[16];
			for (var i = 0; i < buffer.Length; i++) buffer[i] = 0x55;
			var fs = new FileStream(_fh, FileAccess.ReadWrite, SERIAL_BUFFER_SIZE, true);
			while (!_dataFiller.CancellationPending) {
				fs.Write(buffer, 0, buffer.Length);
				fs.Flush();
			}
			fs.Close();
		}

		void data_filler_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			if (_fh != null) {
				if (!_fh.IsInvalid && !_fh.IsClosed) _fh.Close();
				_fh = null;			
			}
			Status = PortStatus.Closed;
		}

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		static extern IntPtr CreateFile(string lpFileName, EFileAccess dwDesiredAccess, EFileShare dwShareMode, IntPtr lpSecurityAttributes,
			 ECreationDisposition dwCreationDisposition, EFileAttributes dwFlagsAndAttributes, IntPtr hTemplateFile);

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("Kernel32.dll", SetLastError = false, CharSet = CharSet.Auto)]
		static extern bool DeviceIoControl(SafeFileHandle hDevice, EIOControlCode IoControlCode,
			[MarshalAs(UnmanagedType.AsAny)] [In] object InBuffer, uint nInBufferSize, out uint OutBuffer,
			uint nOutBufferSize, ref uint pBytesReturned, IntPtr lpOverlapped);

		[DllImport("kernel32.dll")]
		static extern bool GetCommState(SafeFileHandle hFile, [In] ref Dcb lpDCB);

		[DllImport("kernel32.dll")]
		static extern bool SetCommState(SafeFileHandle hFile, [In] ref Dcb lpDCB);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool SetCommTimeouts(SafeFileHandle hFile, [In] ref Commtimeouts lpCommTimeouts);
	}
}
