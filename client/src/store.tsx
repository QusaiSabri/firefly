import { create } from 'zustand';
import { devtools } from 'zustand/middleware';

interface AppState {
  user: { name: string; email: string } | null;
  theme: 'light' | 'dark' | 'system';
  setUser: (user: { name: string; email: string }) => void;
  setTheme: (theme: 'light' | 'dark' | 'system') => void;
  generateImageRequest: {
    contentClass?: string;
    customModelId?: string;
    negativePrompt?: string;
    numVariations?: number;
    prompt: string;
    promptBiasingLocaleCode?: string;
    seeds?: number[];
    size?: { height: number; width: number };
    structure?: {
      imageReference?: {
        source?: { uploadId: string; url?: string };
      };
      strength?: number;
    };
    style?: {
      imageReference?: {
        source?: { uploadId: string; url?: string };
      };
      presets?: string[];
      strength?: number;
    };
    visualIntensity?: number;
  };
  setGenerateImageRequest: (
    data: Partial<AppState['generateImageRequest']>
  ) => void;
}

export const useStore = create<AppState>()(
  devtools((set) => ({
    user: null,
    theme: 'system',
    setUser: (user) => set({ user }),
    setTheme: (theme) => set({ theme }),
    generateImageRequest: {
      prompt: '',
      contentClass: 'photo',
    },
    setGenerateImageRequest: (data) =>
      set((state) => {
        const prev = state.generateImageRequest;

        return {
          generateImageRequest: {
            ...prev,
            ...data,
            structure: data.structure
              ? {
                  ...prev.structure,
                  ...data.structure,
                  imageReference: {
                    ...prev.structure?.imageReference,
                    ...data.structure.imageReference,
                  },
                }
              : prev.structure,
            style: data.style
              ? {
                  ...prev.style,
                  ...data.style,
                  imageReference: {
                    ...prev.style?.imageReference,
                    ...data.style.imageReference,
                  },
                }
              : prev.style,
          },
        };
      }),
  }))
);
