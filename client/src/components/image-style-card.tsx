import * as React from 'react';

import { Button } from '@/components/ui/button';
import { Slider } from '@/components/ui/slider';
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import { UploadIcon } from 'lucide-react';
import { GenerateImageRequest, UploadImageResponse } from '@/models/apiModels';
import { uploadImageAsync } from '@/api/Images';
import { useStore } from '@/store';

export function ImageStyleCard() {
  const referenceImageId = useStore(
    (state) =>
      state.generateImageRequest.style?.imageReference?.source?.uploadId
  );

  const strength = useStore(
    (state) => state.generateImageRequest.style?.strength ?? 50
  );

  const setGenerateImageRequest = useStore(
    (state) => state.setGenerateImageRequest
  );

  const handleImageUpload = async (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const file = event.target.files?.[0];
    if (!file) return;

    try {
      const response: UploadImageResponse = await uploadImageAsync(file);
      setGenerateImageRequest({
        style: {
          imageReference: {
            source: { uploadId: response.images[0].id },
          },
          strength: strength,
        },
      });
    } catch (error) {
      console.error('Failed to upload image', error);
    }
  };

  const disableStrengthSlider = useStore(
    (state) =>
      !state.generateImageRequest.style?.imageReference?.source?.uploadId
  );

  return (
    <Card className='w-[350px] last:mb-0'>
      <CardHeader>
        <CardTitle>Style</CardTitle>
        <CardDescription>A reference image</CardDescription>
      </CardHeader>
      <CardContent>
        <form>
          <div className='grid w-full items-center gap-4'></div>
          <label
            htmlFor='strength'
            className='text-sm font-medium leading-none '
          >
            Strength
          </label>
          {!disableStrengthSlider && (
            <span className='text-sm text-muted-foreground px-2'>
              {strength}
            </span>
          )}
          <Slider
            disabled={disableStrengthSlider}
            id='strength'
            title='How strictly Firefly should adhere to the style you provide. 0 means no adherence. 100 means full adherence.'
            value={[strength]}
            max={100}
            step={1}
            className='w-full py-2'
            onValueChange={(value) => {
              setGenerateImageRequest({
                style: {
                  strength: value[0],
                },
              });
            }}
          />
        </form>
      </CardContent>
      <CardFooter className='flex justify-between'>
        <input
          id='upload-image-style'
          type='file'
          accept='image/*'
          className='hidden'
          onChange={handleImageUpload}
        />
        <label htmlFor='upload-image' title='Upload A Reference Image'>
          <Button
            type='button'
            variant='outline'
            onClick={() =>
              document.getElementById('upload-image-style')?.click()
            }
          >
            <UploadIcon className='mr-1 h-4 w-2' />
            Upload
          </Button>
        </label>
        {referenceImageId && (
          <span className='text-sm text-muted-foreground pl-2 px-1'>
            Image uploaded successfully!
          </span>
        )}
      </CardFooter>
    </Card>
  );
}
