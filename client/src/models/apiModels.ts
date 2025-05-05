export interface GenerateImageRequest {
  prompt: string;
  contentClass?: string;
  customModelId?: string;
  negativePrompt?: string;
  numVariations?: number;
  promptBiasingLocaleCode?: string;
  seeds?: number[];
  size?: FireflyImageSize;
  structure?: StructureSettings;
  style?: StyleSettings;
  visualIntensity?: number;
}

export interface FireflyImageSize {
  width?: number;
  height?: number;
}

export interface StructureSettings {
  imageReference?: ImageReference;
  strength?: number;
}

export interface StyleSettings {
  imageReference?: ImageReference;
  presets?: string[];
  strength?: number;
}

export interface ImageReference {
  source?: UploadSource;
}

export interface UploadSource {
  uploadId?: string;
  url?: string;
}

export interface GenerateImageResponse {
  jobId: string;
  statusUrl: string;
  cancelUrl: string;
}

export interface ImagesResponse {
  urls: string[];
}

export interface isJobCompleteResponse {
  completed: boolean;
}

export interface UploadImageResponse {
  images: UploadedImage[];
}

export interface UploadedImage {
  id: string;
}
