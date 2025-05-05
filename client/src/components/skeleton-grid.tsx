import { Skeleton } from '@/components/ui/skeleton';

export function SkeletonCard() {
  return (
    <div className='flex gap-4 flex-wrap'>
      <Skeleton className='h-[245px] w-[450px] rounded-xl mb-4' />
      <Skeleton className='h-[245px] w-[450px] rounded-xl mb-4' />
      <Skeleton className='h-[245px] w-[450px] rounded-xl mb-4' />
      <Skeleton className='h-[245px] w-[450px] rounded-xl mb-4' />
      <Skeleton className='h-[245px] w-[450px] rounded-xl mb-4' />
      <Skeleton className='h-[245px] w-[450px] rounded-xl mb-4' />
      <Skeleton className='h-[245px] w-[450px] rounded-xl mb-4' />
      <Skeleton className='h-[245px] w-[450px] rounded-xl mb-4' />
      <Skeleton className='h-[245px] w-[450px] rounded-xl mb-4' />
    </div>
  );
}
