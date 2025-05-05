import * as React from 'react';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';
import { generateImageAsync, uploadImageAsync } from '@/api/Images';
import {
  GenerateImageRequest,
  GenerateImageResponse,
  UploadImageResponse,
} from '@/models/apiModels';
import { Message } from '@/models/interfaces';
import { pollForImages } from '@/helpers/poller';
import { UploadIcon } from 'lucide-react';
import { useStore } from '@/store';

export function Chat() {
  const generateImageRequest = useStore((state) => state.generateImageRequest);

  const structureReferenceImageId = useStore(
    (state) =>
      state.generateImageRequest.structure?.imageReference?.source?.uploadId
  );
  // const styleReferenceImageId = useStore(
  //   (state) =>
  //     state.generateImageRequest.style?.imageReference?.source?.uploadId
  // );
  const setGenerateImageRequest = useStore(
    (state) => state.setGenerateImageRequest
  );

  const structureImageStrength = useStore(
    (state) => state.generateImageRequest.structure?.strength
  );

  // const styleImageStrength = useStore(
  //   (state) => state.generateImageRequest.style?.strength
  // );

  const [messages, setMessages] = React.useState<Message[]>([
    {
      id: 1,
      text: 'Hey! I can help you generate images based on your prompts.',
      sender: 'bot',
    },
  ]);
  const [input, setInput] = React.useState('');

  React.useEffect(() => {
    const newMessagesWithJobId = messages.filter(
      (message) => message.jobId && !message.images
    );

    if (newMessagesWithJobId.length > 0) {
      const stopPolling = pollForImages(messages, setMessages);
      return () => stopPolling();
    }
  }, [messages]);

  const handleSend = async () => {
    if (input.trim() === '') return;

    setMessages((prev) => [
      ...prev,
      { id: Date.now(), text: input, sender: 'user' },
    ]);

    try {
      setGenerateImageRequest({ prompt: input });

      const request: GenerateImageRequest = {
        ...generateImageRequest,
        prompt: input,
      };
      //{
      // prompt: input,
      // structure: {
      //   imageReference: {
      //     source: {
      //       uploadId: structureReferenceImageId,
      //     },
      //   },
      //   strength: structureImageStrength,
      // },
      // style: {
      //   imageReference: {
      //     source: {
      //       uploadId: styleReferenceImageId,
      //     },
      //   },
      //   strength: styleImageStrength,
      // },
      //  };

      console.log('Sending request:', request);
      const response: GenerateImageResponse = await generateImageAsync(request);

      setMessages((prev) => [
        ...prev,
        {
          id: Date.now(),
          jobId: response.jobId,
          text: response.jobId ? 'Processing...' : '',
          sender: 'bot',
        },
      ]);
    } catch (error) {
      console.error('Failed to send message', error);
    }

    setInput('');
  };

  const handleImageUpload = async (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const file = event.target.files?.[0];
    if (!file) return;

    try {
      const response: UploadImageResponse = await uploadImageAsync(file);
      setGenerateImageRequest({
        structure: {
          imageReference: {
            source: { uploadId: response.images[0].id },
          },
          strength: structureImageStrength,
        },
      });

      setMessages((prev) => [
        ...prev,
        {
          id: Date.now(),
          text: 'Image uploaded successfully!',
          sender: 'bot',
        },
      ]);
    } catch (error) {
      console.error('Failed to upload image', error);
      setMessages((prev) => [
        ...prev,
        {
          id: Date.now(),
          text: 'Failed to upload image.',
          sender: 'bot',
        },
      ]);
    }
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter') {
      handleSend();
    }
  };

  return (
    <div className='flex flex-col h-full w-[95%] mx-auto border rounded-md shadow-md'>
      {/* <div className='p-2 bg-gray-800 text-white'>
        <Label>Reference Image ID: {structureReferenceImageId}</Label>
      </div> */}
      <div className='flex-1 overflow-y-auto p-4 space-y-4 max-h-[calc(100vh-460px)]'>
        {messages.map((message) => (
          <div key={message.id} className='p-2 rounded-md max-w-fit'>
            {message.images ? (
              <div className='grid grid-cols-2 gap-2'>
                {message.images.map((url, index) => (
                  <img
                    key={index}
                    src={url}
                    alt={`Generated ${index}`}
                    className='w-full h-auto rounded-md'
                  />
                ))}
              </div>
            ) : (
              <span
                className={
                  message.sender === 'bot'
                    ? 'bg-gray-700 self-start p-2 rounded-md'
                    : 'bg-indigo-500 text-white self-end p-2 rounded-md'
                }
              >
                {message.text}
              </span>
            )}
          </div>
        ))}
      </div>
      <div className='flex items-center p-2 border-t gap-2 '>
        <Input
          className='flex-1 mr-2'
          placeholder='Type a message...'
          value={input}
          onChange={(e) => setInput(e.target.value)}
          onKeyDown={handleKeyDown}
        />
        <Button onClick={handleSend}>Send</Button>
        <input
          id='upload-image'
          type='file'
          accept='image/*'
          className='hidden'
          onChange={handleImageUpload}
        />

        <label htmlFor='upload-image' title='Upload A Reference Image'>
          <Button
            type='button'
            variant='outline'
            onClick={() => document.getElementById('upload-image')?.click()}
          >
            <UploadIcon className='mr-1 h-4 w-2' />
            Upload
          </Button>
        </label>
      </div>
    </div>
  );
}
