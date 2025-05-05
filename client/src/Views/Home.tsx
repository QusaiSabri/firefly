import { Chat } from '@/components/chat';
import { ToggleGroupBar } from '@/components/toggle-group-bar';
import { ImageStructureCard } from '@/components/image-structure-card';
import { ImageStyleCard } from '@/components/image-style-card';
import React from 'react';
import { ImageContentClassCard } from '@/components/image-content-class-card';
import GenerateBulk from '@/components/generate-bulk';

const Home: React.FC = () => {
  return (
    <div className='flex flex-1 flex-col gap-4 p-4'>
      <div className='grid auto-rows-min gap-4 md:grid-cols-3'>
        <div className='aspect-video rounded-xl bg-muted/50'>
          <GenerateBulk />
        </div>
        <div className='aspect-video rounded-xl bg-muted/50'>
          <img
            className='bg-contain'
            src='https://fireflymvp.blob.core.windows.net/fireflymvp/625280043_456ab63d-25d1-4990-ac7e-6617fa651b62.jpg?sp=r&st=2025-05-05T07:51:51Z&se=2025-05-09T15:51:51Z&sv=2024-11-04&sr=b&sig=npILLL7i%2Fni0u8%2BxVhII3IiZcrxtl7%2Fes%2BBoFbQBLp4%3D'
          ></img>
        </div>
        <div className='aspect-video rounded-xl bg-muted/50'>
          <img
            className='bg-contain'
            src='https://fireflymvp.blob.core.windows.net/fireflymvp/838064907_a6e80b4e-f1c2-4b36-b6f3-cc89b59270aa.jpg?sp=r&st=2025-05-05T07:56:04Z&se=2025-05-09T15:56:04Z&sv=2024-11-04&sr=b&sig=ahiZ6vsjA%2BSEdQGAnBghNGZOxfmiJ%2BWenBciNEKJPD0%3D'
          ></img>
        </div>
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
