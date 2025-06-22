import React from 'react';
import { useNavigate } from 'react-router-dom';

type GenericLinkProps = {
  to: string;
  text: string;
  className?: string;
} & React.AnchorHTMLAttributes<HTMLAnchorElement>;

const GenericLink: React.FC<GenericLinkProps> = ({ to, text, className = '', ...rest }) => {
  const navigate = useNavigate();

  const handleClick = (e: React.MouseEvent<HTMLAnchorElement, MouseEvent>) => {
    e.preventDefault();
    navigate(to);
  };

  return (
    <a
      href={to}
      onClick={handleClick}
      className={`text-link_text hover:text-link_hover text-sm font-medium cursor-pointer ${className}`}
      tabIndex={0}
      role="link"
      {...rest}
    >
      {text}
    </a>
  );
};

export default GenericLink;
