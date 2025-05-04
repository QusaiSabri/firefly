import { ToggleGroup, ToggleGroupItem } from '@/components/ui/toggle-group';

export function ToggleGroupBar() {
  return (
    <ToggleGroup
      type='single'
      className='flex items-center justify-between mx-auto w-[80%] p-4 rounded-md gap-1'
      defaultValue='bold'
    >
      <ToggleGroupItem
        className='h-8 w-14 rounded-md'
        value='bold'
        aria-label='Toggle bold'
      >
        <h3>Generate Image</h3>
      </ToggleGroupItem>
      <ToggleGroupItem
        value='italic'
        className='h-8 w-14 rounded-md'
        aria-label='Toggle italic'
      >
        <h3>Expand Image</h3>
      </ToggleGroupItem>
      <ToggleGroupItem value='underline' aria-label='Toggle underline'>
        <h3>Fill Image</h3>
      </ToggleGroupItem>
    </ToggleGroup>
  );
}
