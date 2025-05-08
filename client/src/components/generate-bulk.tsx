import React, { useState } from 'react';
import { GenerateImageRequest } from '@/models/apiModels';
import { Button } from '@/components/ui/button';
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
import { AlertMessage } from './alert-message';
import { validate } from 'jsonschema';

const schema = {
  type: 'array',
  items: {
    type: 'object',
    properties: {
      prompt: { type: 'string' },
      size: {
        type: 'object',
        properties: {
          width: { type: 'number' },
          height: { type: 'number' },
        },
        required: ['width', 'height'],
      },
      structure: {
        type: 'object',
        properties: {
          strength: { type: 'number' },
          imageReference: {
            type: 'object',
            properties: {
              source: {
                type: 'object',
                properties: {
                  uploadId: { type: 'string' },
                },
                required: ['uploadId'],
              },
            },
          },
        },
      },
      style: {
        type: 'object',
        properties: {
          strength: { type: 'number' },
          imageReference: {
            type: 'object',
            properties: {
              source: {
                type: 'object',
                properties: {
                  uploadId: { type: 'string' },
                },
                required: ['uploadId'],
              },
            },
          },
          presets: { type: 'array' },
        },
      },
      seeds: {
        type: 'array',
        items: { type: 'number' },
      },
      visualIntensity: { type: 'number' },
    },
    required: ['prompt'],
  },
};

const GenerateBulk: React.FC = () => {
  const [file, setFile] = useState<File | null>(null);
  const [isValid, setIsValid] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [isProcessing, setIsProcessing] = useState(false);
  const [fileCountsMessage, setFileCountsMessage] = useState<string | null>(
    null
  );

  const validateJson = (data: unknown): boolean => {
    console.log('Validating JSON:', data);
    const result = validate(data, schema);
    console.log('Validation result:', result);
    if (!result.valid) {
      setError(result.errors.map((err) => err.stack).join('; '));
      return false;
    }
    setError(null);
    return true;
  };

  const handleFileUpload = async (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const uploadedFile = event.target.files?.[0];
    if (!uploadedFile) {
      setError('No file selected.');
      return;
    }

    setFile(uploadedFile);
    setIsValid(false);

    const reader = new FileReader();
    reader.onload = (e) => {
      const fileContent = e.target?.result;
      if (!fileContent) {
        setError('File is empty or could not be read.');
        return;
      }

      try {
        const json = JSON.parse(fileContent as string);
        if (validateJson(json)) {
          setIsValid(true);
        } else {
          setError('File is not valid JSON.');
        }
      } catch (err) {
        setError(
          'Invalid JSON format. Please ensure the file contains valid JSON.'
        );
      }
    };

    reader.onerror = () => {
      setError('An error occurred while reading the file.');
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
        <CardTitle>Generate Images in Bulk</CardTitle>
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
        {!isValid && error && (
          <span className='flex gap-2 text-sm text-muted-foreground pl-2 px-1'>
            <AlertMessage message='File is not a valid JSON.' />
          </span>
        )}
        {fileCountsMessage && (
          <span className='flex gap-2 text-sm text-muted-foreground pl-2 px-1'>
            <CheckIcon className='mr-1 h-4 w-2' />
            {fileCountsMessage}
          </span>
        )}
      </CardContent>
      <CardFooter className='flex justify-between mt-auto'>
        <input
          id='upload-image-bulk'
          type='file'
          accept='.json'
          className='hidden'
          onChange={handleFileUpload}
        />
        <label htmlFor='upload-image-bulk' title='Upload A JSON File'>
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
