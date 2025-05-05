import { Chat } from '@/components/chat';
import { ToggleGroupBar } from '@/components/toggle-group-bar';
import { ImageStructureCard } from '@/components/image-structure-card';
import { ImageStyleCard } from '@/components/image-style-card';
import React from 'react';
import { ImageContentClassCard } from '@/components/image-content-class-card';

const Home: React.FC = () => {
  return (
    <div className='flex flex-1 flex-col gap-4 p-4'>
      <div className='grid auto-rows-min gap-4 md:grid-cols-3'>
        <div className='aspect-video rounded-xl bg-muted/50' />
        <div className='aspect-video rounded-xl bg-muted/50' />
        <div className='aspect-video rounded-xl bg-muted/50' />
      </div>
      <div className='rounded-xl bg-muted/50' />
      <div className='flex flex-1 gap-4'>
        <div className='flex flex-col gap-y-3 last:mb-0'>
          <ImageContentClassCard />
          <ImageStructureCard />
          <ImageStyleCard />
        </div>
        <div className='flex-1 flex flex-col'>
          <ToggleGroupBar />
          <Chat />
        </div>
      </div>
    </div>
  );
};

export default Home;
