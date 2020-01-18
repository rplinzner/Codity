import { Error } from '../types/base-response';
import { toast } from 'react-toastify';

export default function displayErrors(errors: Error[]): void {
  try {
    errors.forEach((element: Error) => {
    toast.error(element.message);
  });
  }
  catch{
    toast.error('Internal server error');
  }
  
}
