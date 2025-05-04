/* eslint-disable @typescript-eslint/no-explicit-any */
import { getImageAsync, GetJobStatus } from '@/api/Images';
import { Message } from '@/models/interfaces';

export const pollForImages = (
  messages: Message[],
  setMessages: React.Dispatch<React.SetStateAction<any[]>>
) => {
  const interval = setInterval(async () => {
    const pendingMessages = messages.filter(
      (message) => message.jobId && !message.images
    );

    for (const message of pendingMessages) {
      try {
        console.log('Polling for images for message:', message);
        if (!message.jobId) continue;
        const isJobComplete = await GetJobStatus(message.jobId);
        console.log(
          'Job status for message:',
          message.jobId,
          isJobComplete.completed
        );
        if (isJobComplete.completed) {
          console.log('Fetching images for message:', message.jobId);
          const response = await getImageAsync(message.jobId);
          if (response.urls && response.urls.length > 0) {
            setMessages((prevMessages) =>
              prevMessages.map((msg) =>
                msg.id === message.id
                  ? { ...msg, images: response.urls, text: '' }
                  : msg
              )
            );
          }
        }
      } catch (error) {
        console.error('Error polling for images:', error);
      }
    }
  }, 4000);

  return () => clearInterval(interval);
};
