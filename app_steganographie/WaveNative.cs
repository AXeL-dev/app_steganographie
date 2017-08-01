
using System;
using System.Runtime.InteropServices;

namespace app_steganographie
{
	public enum WaveFormats
	{
		Pcm = 1,
		Float = 3
	}

    [StructLayout(LayoutKind.Sequential)] // sont disposés séquentiellement, dans l'ordre dans lequel ils apparaissent lorsqu'il est exporté vers la mémoire non managée.
	public class WaveFormat
	{
        public short wFormatTag; //format de codage des donnees, si PCM donc =1 ^^
		public short nChannels;
        public int nSamplesPerSec;// nbr echantillon par sec= frequence d echantillonage :D 
        public int nAvgBytesPerSec;//nbr d'octet par secondre
        public short nBlockAlign; //nbr octet/echantillon
        public short wBitsPerSample;//nbr de bit/echantillon
        public short cbSize;// taille des donnees

        public WaveFormat(int rate, int bits, int channels)//rate= la frequence :p , bits=nbr bits/echantillon
		{
            wFormatTag = (short)WaveFormats.Pcm; // PCM
            nChannels = (short)channels;// nbr d channel
            nSamplesPerSec = rate; // frequence 
            wBitsPerSample = (short)bits;// nbr de bit /sample
            cbSize = 0; // taille donnee=0

            nBlockAlign = (short)(channels * (bits / 8));//nbr octet par echantillon= nbr de bit par echantiilon div 8 * nbr de channels
            nAvgBytesPerSec = nSamplesPerSec * nBlockAlign;//nbr octet par seconde=nbr octet par echantillon*frequence
		}
	}

	internal class WaveNative
	{
		// consts
		public const int MMSYSERR_NOERROR = 0; // no error

		public const int MM_WOM_OPEN = 0x3BB;
		public const int MM_WOM_CLOSE = 0x3BC;
		public const int MM_WOM_DONE = 0x3BD;

		public const int MM_WIM_OPEN = 0x3BE;
		public const int MM_WIM_CLOSE = 0x3BF;
		public const int MM_WIM_DATA = 0x3C0;

		public const int CALLBACK_FUNCTION = 0x00030000;    // dwCallback is a FARPROC 

		public const int TIME_MS = 0x0001;  // time in milliseconds 
		public const int TIME_SAMPLES = 0x0002;  // number of wave samples 
		public const int TIME_BYTES = 0x0004;  // current byte offset 

		// callbacks
		public delegate void WaveDelegate(IntPtr hdrvr, int uMsg, int dwUser, ref WaveHdr wavhdr, int dwParam2);

		// structs 

		[StructLayout(LayoutKind.Sequential)] public struct WaveHdr
		{
			public IntPtr lpData; // pointer to locked data buffer
			public int dwBufferLength; // length of data buffer
			public int dwBytesRecorded; // used for input only
			public IntPtr dwUser; // for client's use
			public int dwFlags; // assorted flags (see defines)
			public int dwLoops; // loop control counter
			public IntPtr lpNext; // PWaveHdr, reserved for driver
			public int reserved; // reserved for driver
		}

		private const string mmdll = "winmm.dll";

		// WaveOut calls
		[DllImport(mmdll)]
		public static extern int waveOutGetNumDevs();
		[DllImport(mmdll)]
		public static extern int waveOutPrepareHeader(IntPtr hWaveOut, ref WaveHdr lpWaveOutHdr, int uSize);
		[DllImport(mmdll)]
		public static extern int waveOutUnprepareHeader(IntPtr hWaveOut, ref WaveHdr lpWaveOutHdr, int uSize);
		[DllImport(mmdll)]
		public static extern int waveOutWrite(IntPtr hWaveOut, ref WaveHdr lpWaveOutHdr, int uSize);
		[DllImport(mmdll)]
		public static extern int waveOutOpen(out IntPtr hWaveOut, int uDeviceID, WaveFormat lpFormat, WaveDelegate dwCallback, int dwInstance, int dwFlags);
		[DllImport(mmdll)]
		public static extern int waveOutReset(IntPtr hWaveOut);
		[DllImport(mmdll)]
		public static extern int waveOutClose(IntPtr hWaveOut);
		[DllImport(mmdll)]
		public static extern int waveOutPause(IntPtr hWaveOut);
		[DllImport(mmdll)]
		public static extern int waveOutRestart(IntPtr hWaveOut);
		[DllImport(mmdll)]
		public static extern int waveOutGetPosition(IntPtr hWaveOut, out int lpInfo, int uSize);
		[DllImport(mmdll)]
		public static extern int waveOutSetVolume(IntPtr hWaveOut, int dwVolume);
		[DllImport(mmdll)]
		public static extern int waveOutGetVolume(IntPtr hWaveOut, out int dwVolume);

		// WaveIn calls
		[DllImport(mmdll)]
		public static extern int waveInGetNumDevs();
		[DllImport(mmdll)]
		public static extern int waveInAddBuffer(IntPtr hwi, ref WaveHdr pwh, int cbwh);
		[DllImport(mmdll)]
		public static extern int waveInClose(IntPtr hwi);
		[DllImport(mmdll)]
		public static extern int waveInOpen(out IntPtr phwi, int uDeviceID, WaveFormat lpFormat, WaveDelegate dwCallback, int dwInstance, int dwFlags);
		[DllImport(mmdll)]
		public static extern int waveInPrepareHeader(IntPtr hWaveIn, ref WaveHdr lpWaveInHdr, int uSize);
		[DllImport(mmdll)]
		public static extern int waveInUnprepareHeader(IntPtr hWaveIn, ref WaveHdr lpWaveInHdr, int uSize);
		[DllImport(mmdll)]
		public static extern int waveInReset(IntPtr hwi);
		[DllImport(mmdll)]
		public static extern int waveInStart(IntPtr hwi);
		[DllImport(mmdll)]
		public static extern int waveInStop(IntPtr hwi);
	}
}
