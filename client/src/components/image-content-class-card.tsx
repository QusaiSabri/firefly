import * as React from 'react';

import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { useStore } from '@/store';
import { ToggleGroup, ToggleGroupItem } from '@radix-ui/react-toggle-group';

export function ImageContentClassCard() {
  const setGenerateImageRequest = useStore(
    (state) => state.setGenerateImageRequest
  );

  return (
    <Card
      className='w-[350px]'
      title='Directs the style of a generated image to be photographic or like fine
          art.'
    >
      <CardHeader>
        <CardTitle>Content Type</CardTitle>
      </CardHeader>
      <CardContent>
        <form>
          <ToggleGroup
            type='single'
            className='flex items-center justify-around mx-auto p-2 rounded-md gap-1'
            value={useStore((state) => state.generateImageRequest.contentClass)}
            onValueChange={(value) => {
              useStore
                .getState()
                .setGenerateImageRequest({ contentClass: value });
            }}
          >
            <ToggleGroupItem
              className='h-8 w-14 rounded-md  data-[state=on]:bg-blue-500 data-[state=on]:text-white'
              value='photo'
              aria-label='Photo'
            >
              <h3>Photo</h3>
            </ToggleGroupItem>
            <ToggleGroupItem
              className='h-8 w-14 rounded-md  data-[state=on]:bg-blue-500 data-[state=on]:text-white'
              value='art'
              aria-label='Art'
            >
              <h3>Art</h3>
            </ToggleGroupItem>
          </ToggleGroup>
        </form>
      </CardContent>
    </Card>
  );
}
