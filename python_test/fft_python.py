import numpy as np
import matplotlib.pyplot as plt
import torch
import torchaudio

# read an audio file using torchaudio
filename = 'chirp'
waveform, sample_rate = torchaudio.load(f'{filename}.wav')

print(f'waveform Shape: {waveform.shape}')
# resample the waveform to 16000 Hz
resample = torchaudio.transforms.Resample(orig_freq=sample_rate, new_freq=16000)
waveform = resample(waveform)
sample_rate = 16000
# waveform = waveform[:,:8000]  # only use the first channel
print(waveform.shape, sample_rate)

# save the waveform as a txt file where each row is a sample in the waveform
np.savetxt(f'{filename}.txt', waveform.numpy(), delimiter='\n')


# mel spectrogram
mel_S = torchaudio.transforms.MelSpectrogram(sample_rate, n_fft=2048, win_length = None,  hop_length=64, n_mels=128, center=False, pad_mode='reflect')
melspectrogram = mel_S(waveform)

print(melspectrogram.shape)

# save the mel spectrogram as a txt file where each row is a sample in the mel spectrogram
np.savetxt(f'torch_mel_spectrogram.txt', melspectrogram.squeeze().numpy())


# Load the spectrogram from the txt file
melspectrogram = np.loadtxt('torch_mel_spectrogram.txt')
# melspectrogram = np.loadtxt('my_mel_spectrogram.txt')
print(f'Torch melspectrogram Shape: {melspectrogram.shape}')
# plot it 
plt.figure()
plt.imshow(melspectrogram, cmap='viridis')
plt.colorbar()
# plt.show()

melspectrogram = np.loadtxt('my_mel_spectrogram.txt')
print(f'My melspectrogram Shape: {melspectrogram.shape}')
# plot it 
plt.figure()
plt.imshow(melspectrogram, cmap='viridis')
plt.colorbar()
plt.show()

