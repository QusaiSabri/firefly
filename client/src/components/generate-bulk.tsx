import React, { useState } from 'react';
import { GenerateImageRequest } from '@/models/apiModels';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Alert } from '@/components/ui/alert';
import { generateImagesBulkAsync } from '@/api/Images';
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from './ui/card';
import { CheckIcon, UploadIcon } from 'lucide-react';

const GenerateBulk: React.FC = () => {
  const [file, setFile] = useState<File | null>(null);
  const [isValid, setIsValid] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [isProcessing, setIsProcessing] = useState(false);
  const [fileCountsMessage, setFileCountsMessage] = useState<string | null>(
    null
  );

  const validateJson = (data: GenerateImageRequest[]): boolean => {
    if (!Array.isArray(data)) {
      setError('Uploaded file must be a JSON array.');
      return false;
    }

    for (const item of data) {
      if (typeof item.prompt !== 'string') {
        setError('Each item must have a valid prompt.');
        return false;
      }

      if (item.size) {
        if (
          typeof item.size.width !== 'number' ||
          typeof item.size.height !== 'number'
        ) {
          setError(
            'Each item must have valid width and height if size is provided.'
          );
          return false;
        }
      }

      if (item.structure) {
        if (
          item.structure.strength &&
          typeof item.structure.strength !== 'number'
        ) {
          setError('Structure strength must be a number if provided.');
          return false;
        }

        if (
          item.structure.imageReference &&
          item.structure.imageReference.source &&
          typeof item.structure.imageReference.source.uploadId !== 'string'
        ) {
          setError(
            'Structure imageReference source uploadId must be a string if provided.'
          );
          return false;
        }
      }

      if (item.style) {
        if (item.style.strength && typeof item.style.strength !== 'number') {
          setError('Style strength must be a number if provided.');
          return false;
        }

        if (
          item.style.imageReference &&
          item.style.imageReference.source &&
          typeof item.style.imageReference.source.uploadId !== 'string'
        ) {
          setError(
            'Style imageReference source uploadId must be a string if provided.'
          );
          return false;
        }

        if (item.style.presets && !Array.isArray(item.style.presets)) {
          setError('Style presets must be an array if provided.');
          return false;
        }
      }

      if (
        item.seeds &&
        (!Array.isArray(item.seeds) ||
          !item.seeds.every((seed) => typeof seed === 'number'))
      ) {
        setError('Seeds must be an array of numbers if provided.');
        return false;
      }

      if (item.visualIntensity && typeof item.visualIntensity !== 'number') {
        setError('Visual intensity must be a number if provided.');
        return false;
      }
    }

    setError(null);
    return true;
  };

  const handleFileUpload = async (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const uploadedFile = event.target.files?.[0];
    if (!uploadedFile) return;

    setFile(uploadedFile);
    setIsValid(false);

    const reader = new FileReader();
    reader.onload = (e) => {
      try {
        const json = JSON.parse(e.target?.result as string);
        if (validateJson(json)) {
          setIsValid(true);
        }
      } catch (err) {
        setError('Invalid JSON file.');
      }
    };
    reader.readAsText(uploadedFile);
  };

  const handleProcess = async () => {
    if (!file || !isValid) return;

    setIsProcessing(true);
    try {
      const reader = new FileReader();
      reader.onload = async (e) => {
        try {
          const json = JSON.parse(
            e.target?.result as string
          ) as GenerateImageRequest[];
          const response = await generateImagesBulkAsync(json);

          console.log('Response:', response);
          const count = response.length;
          if (count > 0) {
            setFileCountsMessage(
              `Successfully submitted ${count} requests.\nCheck the Library tab for results in a few minutes.`
            );
          }
        } catch (err) {
          console.error('Error processing file:', err);
          alert('An error occurred while processing the file.');
        } finally {
          setIsProcessing(false);
        }
      };
      reader.readAsText(file);
    } catch (err) {
      console.error('Error reading file:', err);
      setIsProcessing(false);
    }
  };

  return (
    <Card className='h-full'>
      <CardHeader>
        <CardTitle>Generate Bulk Images</CardTitle>
        <CardDescription>Upload a json file..</CardDescription>
      </CardHeader>
      <CardContent>
        <form>
          <div className='grid w-full items-center gap-4'></div>
        </form>
        {isValid && !fileCountsMessage && (
          <span className='flex gap-2 text-sm text-muted-foreground pl-2 px-1'>
            <CheckIcon className='mr-1 h-4 w-2' />
            File is valid
          </span>
        )}
        {fileCountsMessage && (
          <span className='flex gap-2 text-sm text-muted-foreground pl-2 px-1'>
            <CheckIcon className='mr-1 h-4 w-2' />
            {fileCountsMessage}
          </span>
        )}
      </CardContent>
      <CardFooter className='flex justify-between'>
        <input
          id='upload-image-bulk'
          type='file'
          accept='.json'
          className='hidden'
          onChange={handleFileUpload}
        />
        <label htmlFor='upload-image-bulk' title='Upload A Reference Image'>
          <Button
            type='button'
            variant='outline'
            onClick={() =>
              document.getElementById('upload-image-bulk')?.click()
            }
          >
            <UploadIcon className='mr-1 h-4 w-2' />
            Upload
          </Button>
        </label>
        <Button onClick={handleProcess} disabled={!isValid || isProcessing}>
          {isProcessing ? 'Processing...' : 'Process'}
        </Button>
      </CardFooter>
    </Card>
  );
};

export default GenerateBulk;
