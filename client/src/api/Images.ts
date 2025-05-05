/* eslint-disable @typescript-eslint/no-explicit-any */
import {
  GenerateImageRequest,
  ImagesResponse,
  isJobCompleteResponse,
  UploadImageResponse,
} from '@/models/apiModels';
import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export const generateImageAsync = async (
  generateImageRequest: GenerateImageRequest
): Promise<any> => {
  const response = await axios.post(
    `${API_BASE_URL}/images/generate-async`,
    generateImageRequest
  );
  return response.data;
};

export const getMessages = async (): Promise<any[]> => {
  const response = await axios.get(`${API_BASE_URL}/messages`);
  return response.data;
};

export const getAllImagesAsync = async (
  limit?: number
): Promise<ImagesResponse> => {
  const response = await axios.get(`${API_BASE_URL}/images`, {
    params: {
      limit,
    },
  });
  return response.data;
};

export const getImageAsync = async (jobId: string): Promise<ImagesResponse> => {
  const response = await axios.get(`${API_BASE_URL}/jobs/${jobId}/images`);
  return response.data;
};

export const GetJobStatus = async (
  jobId: string
): Promise<isJobCompleteResponse> => {
  const response = await axios.get(`${API_BASE_URL}/jobs/${jobId}/status`);
  return response.data;
};

export const uploadImageAsync = async (
  file: File
): Promise<UploadImageResponse> => {
  const formData = new FormData();
  formData.append('file', file);

  const response = await axios.post(
    `${API_BASE_URL}/images/upload/firefly`,
    formData,
    {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    }
  );

  return response.data;
};
