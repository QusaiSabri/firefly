import React, { useEffect, useState } from 'react';
import { getAllImagesAsync } from '@/api/Images';
import { ImagesResponse } from '@/models/apiModels';
import { SkeletonCard } from '@/components/skeleton-grid';

const Library: React.FC = () => {
  const [images, setImages] = useState<string[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchImages = async () => {
      try {
        const response: ImagesResponse = await getAllImagesAsync();
        setImages(response.urls);
        setLoading(false);
      } catch (error) {
        console.error('Error fetching images:', error);
      }
    };

    fetchImages();
  }, []);

  return (
    <div className='p-4'>
      <h1 className='text-2xl font-bold mb-4'>Library</h1>
      {loading && <SkeletonCard />}
      <div className='columns-2 md:columns-3 lg:columns-4 gap-4'>
        {images.map((url, index) => (
          <div key={index} className='mb-4 break-inside-avoid'>
            <img
              src={url}
              alt={`Image ${index + 1}`}
              className='w-full rounded-lg shadow-md hover:shadow-lg transition-shadow duration-300'
            />
          </div>
        ))}
      </div>
    </div>
  );
};

export default Library;
