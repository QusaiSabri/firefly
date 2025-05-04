import * as React from 'react';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';
import { generateImageAsync } from '@/api/Images';
import { GenerateImageResponse } from '@/models/apiModels';
import { Message } from '@/models/interfaces';
import { pollForImages } from '@/helpers/poller';

export function Chat() {
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
      const response: GenerateImageResponse = await generateImageAsync({
        prompt: input,
      });

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

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter') {
      handleSend();
    }
  };

  return (
    <div className='flex flex-col h-full w-[80%] mx-auto border rounded-md shadow-md'>
      <div className='flex-1 overflow-y-auto p-4 space-y-4 max-h-[calc(100vh-542px)]'>
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
                    ? 'bg-gray-700 self-start'
                    : 'bg-indigo-500 text-white self-end'
                }
              >
                {message.text}
              </span>
            )}
          </div>
        ))}
      </div>
      <div className='flex items-center p-2 border-t'>
        <Input
          className='flex-1 mr-2'
          placeholder='Type a message...'
          value={input}
          onChange={(e) => setInput(e.target.value)}
          onKeyDown={handleKeyDown}
        />
        <Button onClick={handleSend}>Send</Button>
      </div>
    </div>
  );
}
