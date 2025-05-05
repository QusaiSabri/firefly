import { ToggleGroup, ToggleGroupItem } from '@/components/ui/toggle-group';

export function ToggleGroupBar() {
  return (
    <ToggleGroup
      type='single'
      className='flex items-center justify-between mx-auto w-[80%] p-4 rounded-md gap-1'
      defaultValue='GenerateImage'
    >
      <ToggleGroupItem
        className='h-8 w-14 rounded-md'
        value='GenerateImage'
        aria-label='Generate Image'
      >
        <h3>Generate Image</h3>
      </ToggleGroupItem>
      <ToggleGroupItem
        value='ExpandImage'
        className='h-8 w-14 rounded-md'
        aria-label='Expand Image'
      >
        <h3>Expand Image</h3>
      </ToggleGroupItem>
      <ToggleGroupItem
        className='h-8 w-14 rounded-md'
        value='FillImage'
        aria-label='Fill Image'
      >
        <h3>Fill Image</h3>
      </ToggleGroupItem>
    </ToggleGroup>
  );
}
