export interface Message {
  id: number;
  text: string;
  sender: 'user' | 'bot';
  jobId?: string;
  images?: string[];
}
